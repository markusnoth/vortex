const net = require('net')

module.exports = vortexClient

const RESPONSE_TYPES = {
    COMMAND_RESPONSE: 1,
    PAGE_REQUEST: 2,
    PAGE_WITH_COMMAND_ROW: 3
}

const RESPONSE_STATUS_CODES = {
    SUCCESS: 3,
    ERROR: 6,
    SUCCESS_TEXT: 7
}

const CONFIG_DEFAULTS = {
    port: 1025,
    timeout: 5000
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
        return this.start(() => {
            const { host, port } = this.config
            this.client.connect(port, host, () => this.onConnected())
        })
    },
    onConnected() {
        const { host, port, username, password } = this.config
    },
    login(username, password) {
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
        const responseComplete = data[data.length - 2] === 0xF8 && data[data.length - 1] === 1
        data = this.data ? this.data.concat(data) : Array.from(data)
        if (responseComplete) {
            const type = data.shift()
            switch (type) {
                case RESPONSE_TYPES.COMMAND_RESPONSE: {
                    data = data.slice(data.findIndex(i => i !== 32), 81)
                    const status = data.filter(i => i === 0x03 || i === 0x06 || i === 0x07)[0]
                    let command = String.fromCharCode.apply(String, data.filter(i => i >= 0x20)).trim()
                    if (status === RESPONSE_STATUS_CODES.ERROR) {
                        throw new Error(command)
                    }
                    return this.end(command)
                }
                case RESPONSE_TYPES.PAGE_WITH_COMMAND_ROW: {
                    return this.end(String.fromCharCode.apply(String, data))
                }
            }
        } else {
            this.data = [...data]
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
