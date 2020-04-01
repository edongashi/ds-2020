// A
const crypto = require('crypto')
const { connect } = require('simple-socket-session')
connect('http://185.67.178.114:3000', session)

async function session(send, receive) {
  await send('emri', 'Filan')

  const alice = crypto.createDiffieHellman(128)
  const p = alice.getPrime('hex')
  const g = alice.getGenerator('hex')
  const A = alice.generateKeys('hex')

  await send('keys', { g, p, A })
  const B = await receive('B')

  const s = alice.computeSecret(B, 'hex', 'hex')

  console.log('Vlerat', {
    p, g, A, B, s
  })

  const message = 'Pershendetje'

  const iv = crypto.randomBytes(8)
  const key = crypto.scryptSync(s, 'salt', 8)
  const cipher = crypto.createCipheriv('des-cbc', key, iv)

  let ciphertext = cipher.update(message, 'utf8', 'hex')
  ciphertext += cipher.final('hex')

  console.log('Ciphertext', ciphertext)
  await send('ciphertext', {
    iv: iv.toString('hex'),
    ciphertext
  })
}
