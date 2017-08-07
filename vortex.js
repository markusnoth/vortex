const net = require('net')
const colors = require('colors')

module.exports = vortexClient

const REQUEST_TYPES = {
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
    port: 1025
}

function vortexClient(config) {
    this.config = Object.assign({}, CONFIG_DEFAULTS, config)
    this.client.on('data', data => this.onData(data))
    this.client.on('close', () => this.onDisconnected())
}

vortexClient.prototype = {
    client: new net.Socket(),
    connect() {
        const { host, port } = this.config
        this.client.connect(port, host, () => this.onConnected())
    },
    onConnected() {
        const { host, port, username, password } = this.config
        console.log(colors.green(`Successfully connected to ${host}:${port}`))
        if (username && password) {
            this.login(username, password)
        }
    },
    login(username, password) {
        this.send(`log ${username}`)
    },
    send(cmd) {
        console.log(`Sending ${cmd}`)
        const data = [0x00, ...cmd.split('').map((c, i) => cmd.charCodeAt(i))]
        for (let i = cmd.length; i <= 80; i++) {
            data.push(0x20)
        }
        data.push(0xF8)
        data.push(0x01)
        this.client.write(new Buffer(data), 'utf8', () => {
            console.log(`Data '${cmd}' successfully sent`)
        })
    },
    onData(data) {
        const type = data[0]
        const dataStart = data.findIndex((e, i) => i > 0 && e !== 32)
        data = data.slice(dataStart, 81)
        const status = data.filter(i => i === 0x03 || i === 0x06 || i === 0x07)[0]
        data = new String(data.filter(i => i >= 0x20)).trim()
        if(status === RESPONSE_STATUS_CODES.ERROR) data = colors.red(data)
        if(status === RESPONSE_STATUS_CODES.SUCCESS) data = colors.green(data)
        console.log(data)
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
