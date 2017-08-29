const express = require('express')
const bodyParser = require('body-parser')
const multer = require('multer')
const config = require('config')
const passport = require('passport')
const BasicStrategy = require('passport-http').BasicStrategy

const VortexClient = require('./vortex')

// setup vortex client
const client = new VortexClient(config.vortex)
client.connect()
    .then(response => console.log(response))
    .catch(error => console.error(error))

// setup express server
const PORT = process.env.PORT || 8080
const app = express()
app.use(passport.initialize())
passport.use(new BasicStrategy((username, password, done) => {
    done(null, { username })
}))
const upload = multer()

function authenticate(req, res, next, callback) {
    return passport.authenticate('basic', { session: false }, callback)(req, res, next)
}

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

app.post('/page/:mag/:set/:page?',
    authenticate,
    bodyParser.raw({
        type: '*/*',
        verify: (req, res, buffer, encoding) => {
            if (buffer.length !== 1008) throw new Error('Invalid content size')
        }
    }),
    (req, res, next) => {
        const { mag, set, page = 1 } = req.params
        let command = `COIN ${mag} ${set}`
        if (page) command += `.${page}`
        return sendPage(req, res, next, command, req.body)
    }
)
app.post('/page',
    upload.single('data'),
    authenticate,
    (req, res, next) => {
        const { command } = req.body
        if (!(command && req.file)) {
            return res.sendStatus(400)
        }
        return sendPage(req, res, next, command, req.file.buffer)
    }
)
function sendPage(req, res, next, command, buffer) {
    const match = command.match(/^([a-z]+) ([0-9]+) ([0-9]+)\.([0-9]+)/i)
    if(!match) {
        return res.sendStatus(400)
    }
    const [_match, cmd, mag, set, page] = match
    if(!(mag === 18 && set === 70 && page === 1)) {
        return res.sendStatus(401)
    }
    const data = []
    for (let i = 0; i < 24; i++) {
        data.push(i)
        const startIdx = 6 + i * 40
        data.push(...buffer.slice(startIdx, startIdx + 40))
    }
    client.sendPage(command, data)
        .then(response => res.send(response))
        .catch(next)
}

app.post('/cmd', authenticate, bodyParser.text(), (req, res, next) => {
    client.sendCommand(req.body)
        .then(result => {
            let { response, page } = result
            if (!response && page) {
                response = String.fromCharCode.apply(String, page)
            }
            if (response) return res.send(response)
            return res.json(result)
        })
        .catch(next)
})

app.use((err, req, res, next) => {
    res.status(500).send(err.message || err)
})

app.listen(PORT, () => {
    console.log(`Server listening on port ${PORT}`)
})
