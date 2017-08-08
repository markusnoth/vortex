const express = require('express')
const config = require('config')

const VortexClient = require('./vortex')

const PORT = process.env.PORT || 8080

const client = new VortexClient(config.vortex)
client.connect()
    .then(response => console.log(response))
    .catch(error => console.error(error))

const app = express()

app.get('/:mag/:set?/:page?', (req, res, next) => {
    client.send(`${req.params.mag}`)
        .then(response => res.send(response))
        .catch(error => res.status(500).send(error.message || error))
})

app.listen(PORT, () => {
    console.log(`Server listening on port ${PORT}`)
})