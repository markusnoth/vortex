const express = require('express')
const bodyParser = require('body-parser')
const multer = require('multer')()
const config = require('config')

const VortexClient = require('./vortex')

// setup vortex client
const client = new VortexClient(config.vortex)
client.connect()
    .then(response => console.log(response))
    .catch(error => console.error(error))

// setup express server
const PORT = process.env.PORT || 8080
const app = express()

// setup url-rewriting for iisnode
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
            res.setHeader('Content-Disposition', `attachment; filename=${mag}_${set || 0}_${page || 0}.ep1`);
            res.setHeader('Content-Type', 'application/ep1')
            res.send(Buffer.from(response))
        })
        .catch(next)
})

app.post('/page', multer.single('data'), (req, res, next) => {
    const { command } = req.body
    if (!(command && req.file)) {
        return res.sendStatus(400)
    }
    const data = []
    for (let i = 0; i < 24; i++) {
        data.push(i)
        const startIdx = 6 + i * 40
        data.push(...req.file.buffer.slice(startIdx, startIdx + 40))
    }
    client.sendPage(command, data)
        .then(response => res.send(response))
        .catch(next)
})

app.post('/cmd', bodyParser.text(), (req, res, next) => {
    client.send(req.body)
        .then(({ response }) => res.send(response))
        .catch(next)
})

app.use((err, req, res, next) => {
    res.status(500).send(err.message || err)
})

app.listen(PORT, () => {
    console.log(`Server listening on port ${PORT}`)
})
