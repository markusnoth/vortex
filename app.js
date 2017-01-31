const net = require('net')

const HOST = '10.100.133.151'
const PORT = 1025

/**
 * console input setup
 */
const stdin = process.openStdin()
stdin.addListener('data', input => {
    const cmd = input.toString().trim()
    console.log(`Sending ${cmd}`)
    const data = [0x00, ...cmd.split('').map((c,i) => cmd.charCodeAt(i))]
    for(let i = cmd.length;i<=80;i++) {
        data.push(0x20)
    }
    data.push(0xF8)
    data.push(0x01)
    client.write(new Buffer(data), 'utf8', () => {
        console.log(`Data '${cmd}' successfully sent`)
    })
})

/**
 * tcp client setup
 */
const client = new net.Socket()
client.connect(PORT, HOST, () => {
    console.log(`Successfully connected to ${HOST}:${PORT}`)
})
client.on('data', function(data) {
    const type = data[0]
    data = data.slice(1, data.length - 2)
    // Success:0x03 Error:0x06 SuccessText:0x07
    const status = data.filter(i => i === 0x03 || i === 0x06 || i === 0x07)[0]
    data = new String(data.filter(i => i >= 0x20)).trim()
	console.log(status + ': ' + data)
	//client.destroy(); // kill client after server's response
})
client.on('close', function() {
	console.log('Connection closed')
})

/**
 * exit handling
 */
function cleanup() {
    client.end()
}
process.on('uncaughtException', (err) => {
    console.error(err)
    cleanup()
    process.exit()
})
process.on('beforeExit', () => {
    cleanup()
})
process.on('SIGINT', function() {
    cleanup()
    setTimeout(() => process.exit(), 1000)
})