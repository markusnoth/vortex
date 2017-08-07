const VortexClient = require('./vortex')

const VORTEX_CONFIG = {
    host: '10.100.133.151',
    port: 1025,
    username: 'reg01',
    password: 'reg01'
}

/**
 * setup vortext client
 */
const client = new VortexClient(VORTEX_CONFIG)
client.connect()

/**
 * console input setup
 */
const stdin = process.openStdin()
stdin.addListener('data', input => {
    const cmd = input.toString().trim()
    client.send(cmd)
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