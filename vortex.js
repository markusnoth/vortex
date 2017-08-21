const net = require('net')

module.exports = vortexClient

const ROW_LENGTH = 40

const RESPONSE_TYPES = {
    COMMAND_RESPONSE: 1,
    PAGE_REQUEST: 2,
    PAGE_WITH_COMMAND_ROW: 3,
    USER_ACCESS_RESPONSE: 34
}

const RESPONSE_STATUS_CODES = {
    SUCCESS: 3,
    ERROR: 6,
    SUCCESS_TEXT: 7
}

const REQUEST_TYPES = {
    COMMAND_LINE: 10,
    PAGE_RESPONSE: 2
}

const CONFIG_DEFAULTS = {
    port: 1025,
    timeout: 5000,
    autoConnect: true
}

function vortexClient(config) {
    this.config = Object.assign({}, CONFIG_DEFAULTS, config)
    this.client.on('data', data => this.onData(data))
    this.client.on('close', () => this.onDisconnected())
}

vortexClient.prototype = {
    client: new net.Socket(),
    start(action) {
        return new Promise((resolve, reject) => {
            const result = action()
            const timeout = setTimeout(() => {
                reject('Timeout')
            }, this.config.timeout)
            Object.assign(this, { resolve, reject, timeout })
            return result
        })
    },
    end(result) {
        if (this.timeout) {
            clearTimeout(this.timeout)
        }
        if (result.error) {
            this.reject(result.error)
        }
        else {
            this.resolve(result.response || result)
        }
        delete this.resolve
        delete this.reject
        delete this.data
    },
    connect() {
        const { host, port, username, password, autoConnect } = this.config
        return this.start(() => this.client.connect(port, host))
            .then(() => {
                if (username && password && autoConnect) {
                    return this.login(username, password)
                }
            })
    },
    login(username, password) {
        if (!username) throw new Error('Username is required')
        if (!password) throw new Error('Password is required')
        return this.send(`log ${username}`).then(response => this.send(password))
    },
    send(cmd) {
        if (typeof cmd !== 'string' || cmd.length === 0) {
            return Promise.reject('Invalid command')
        }
        return this.start(() => {
            return new Promise((resolve, reject) => {
                const data = Buffer.alloc(83, 0x20)
                data[0] = REQUEST_TYPES.COMMAND_LINE
                data.write(cmd, 1)
                data[81] = 0xF8
                data[82] = 0x01
                this.client.write(data, () => resolve(cmd))
            })
        })
    },
    onData(data) {
        data = Array.from(data)
        if (this.data) {
            data = this.data.concat(data)
        }
        const responseComplete = data[data.length - 2] === 0xF8 && data[data.length - 1] === 1
        if (responseComplete) {
            data.splice(-2)
            const type = data.shift()
            switch (type) {
                case RESPONSE_TYPES.COMMAND_RESPONSE: {
                    const [status, ...command] = data.slice(data.findIndex(i => i !== 32), 81)
                    const commandText = String.fromCharCode.apply(String, command.filter(i => i >= 0x20)).trim()
                    if (status === RESPONSE_STATUS_CODES.ERROR) {
                        return this.end({ error: commandText })
                    }
                    return this.end({ type, response: commandText })
                }
                case RESPONSE_TYPES.PAGE_WITH_COMMAND_ROW: {
                    const page = new Array(ROW_LENGTH * 24)
                    for (let row = data.shift(); row !== 255; row = data.shift()) {
                        page.splice(row * ROW_LENGTH, ROW_LENGTH, ...data.splice(0, ROW_LENGTH))
                    }
                    const command = data.slice(data.findIndex(i => i !== 32), 81)
                    return this.end({ type, page, command })
                }
                case RESPONSE_TYPES.PAGE_REQUEST: {
                    return this.end({ type })
                }
                case RESPONSE_TYPES.USER_ACCESS_RESPONSE: {
                    // explicitely don't handle this response, as the password is sent implicitely
                    return
                }
                default: {
                    return this.end({ error: `Unhandled response type ${type}` })
                }
            }
        } else {
            this.data = data
        }
    },
    getPage(mag, set, page) {
        let command = new String(mag)
        if (set || page) command += ' ' + set
        if (page) command += '.' + page
        return this.send(command).then(response => {
            if (response.type === RESPONSE_TYPES.PAGE_WITH_COMMAND_ROW) {
                const { page, command } = response
                page.splice(0, 0, 0xFE, 0x01, 0x1A, 0x00, 0x00, 0x00)
                page.push(...new Array(42))
                return page
            }
            throw new Error(`Invalid response: ${response}`)
        })
    },
    sendPage(command, data) {
        return this.send(command)
            .then(response => {
                if (response.type !== RESPONSE_TYPES.PAGE_REQUEST) {
                    return Promise.reject(`Unexpected response ${response}`)
                }
                return this.start(() => {
                    data = Buffer.from(data)
                    const buffer = Buffer.alloc(data.length + 3)
                    buffer[0] = REQUEST_TYPES.PAGE_RESPONSE
                    data.copy(buffer, 1)
                    buffer[buffer.length - 2] = 0xF8
                    buffer[buffer.length - 1] = 0x01
                    this.client.write(buffer)
                })
            })
    },
    disconnect() {
        try {
            this.client.end()
        } catch (err) {
            console.error('Failed to disconnect properly from Vortex.')
        }
    },
    onDisconnected() {
        console.log('Connection closed')
    },
}
