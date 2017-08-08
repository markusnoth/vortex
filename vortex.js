const net = require('net')

module.exports = vortexClient

const ROW_LENGTH = 40

const RESPONSE_TYPES = {
    COMMAND_RESPONSE: 1,
    PAGE_WITH_COMMAND_ROW: 3
}

const RESPONSE_STATUS_CODES = {
    SUCCESS: 3,
    ERROR: 6,
    SUCCESS_TEXT: 7
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
                throw new Error('Timeout')
            }, this.config.timeout)
            Object.assign(this, { resolve, reject, timeout })
            return result
        })
    },
    end(result, error) {
        if (this.timeout) {
            clearTimeout(this.timeout)
        }
        if (error) {
            this.reject(error)
        }
        if (this.resolve) {
            this.resolve(result)
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
        return this.start(() => {
            return new Promise((resolve, reject) => {
                const data = [0x00, ...cmd.split('').map((c, i) => cmd.charCodeAt(i))]
                for (let i = cmd.length; i <= 80; i++) {
                    data.push(0x20)
                }
                data.push(0xF8, 0x01)
                this.client.write(new Buffer(data), 'utf8', () => resolve(cmd))
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
                        throw new Error(commandText)
                    }
                    return this.end(commandText)
                }
                case RESPONSE_TYPES.PAGE_WITH_COMMAND_ROW: {
                    const page = new Array(ROW_LENGTH * 24)
                    for (let row = data.shift(); row !== 255; row = data.shift()) {
                        page.splice(row * ROW_LENGTH, ROW_LENGTH, ...data.splice(0, ROW_LENGTH))
                    }
                    const command = data.slice(data.findIndex(i => i !== 32), 81)
                    return this.end(page)
                }
            }
        } else {
            this.data = data
        }
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
