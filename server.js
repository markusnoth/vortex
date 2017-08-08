const express = require('express')
const config = require('config')

const VortexClient = require('./vortex')

const PORT = process.env.PORT || 8080

const client = new VortexClient(config.vortex)
client.connect()
    .then(response => console.log(response))
    .catch(error => console.error(error))

const app = express()

// SETUP URL-REWRITING FOR IISNODE
if (process.env.VIRTUAL_DIR_PATH) {
    app.use((req, res, next) => {
        req.url = req.url.replace(new RegExp('^' + process.env.VIRTUAL_DIR_PATH), '')
        next()
    })
}

app.get('/page/:mag/:set?/:page?', (req, res, next) => {
    const { mag, set, page } = req.params
    client.getPage(mag, set, page)
        .then(response => {
            res.setHeader('Content-Type', 'application/ep1')
            res.send(Buffer.from(response))
        })
        .catch(error => res.status(500).send(error.message || error))
})

app.listen(PORT, () => {
    console.log(`Server listening on port ${PORT}`)
})