const config = require('config')
const colors = require('colors')

const VortexClient = require('./vortex')

/**
 * setup vortext client
 */
const client = new VortexClient(config.vortex)
const { host, port, username, password } = config.vortex
client.connect().then(message => console.log(colors.green(message)))

/**
 * console input setup
 */
const stdin = process.openStdin()
stdin.addListener('data', input => {
    const cmd = input.toString().trim()
    client.send(cmd).then(response => {
        if (Array.isArray(response) && response.length === 960) {
            while (response.length > 0) {
                let row = response.splice(0, 40).map(i => i >= 0x20 ? i : 0x20)
                row = String.fromCharCode.apply(String, row)
                row = colors.bgBlack(colors.white(row + ' '))
                console.log(row)
            }
        } else {
            console.log(response)
        }
    })
})

/**
 * exit handling
 */
function cleanup() {
    client.disconnect()
}
process.on('uncaughtException', (err) => {
    console.error(err)
    cleanup()
    process.exit()
})
process.on('beforeExit', () => {
    cleanup()
})
process.on('SIGINT', function () {
    cleanup()
    setTimeout(() => process.exit(), 1000)
})