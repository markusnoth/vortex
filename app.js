const net = require('net')

const HOST = '10.100.133.151'
const PORT = 1025

const client = new net.Socket()

client.connect(PORT, HOST, () => {
    console.log(`Successfully connected to ${HOST}:${PORT}`)
})

client.on('data', function(data) {
	console.log('Received: ' + data);
	//client.destroy(); // kill client after server's response
})

client.on('close', function() {
	console.log('Connection closed');
})

const stdin = process.openStdin()

stdin.addListener('data', data => {
    console.log(`Sending ${data}`, typeof data)
    client.write(data, 'utf8', () => {
        console.log(`Data '${data}' successfully sent`)
    })
})