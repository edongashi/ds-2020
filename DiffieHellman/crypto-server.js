// B
const crypto = require('crypto')
const { listen } = require('simple-socket-session')
listen(session)

async function session(send, receive) {
  const emri = await receive('emri')
  console.log('Konektim me', emri)

  const { g, p, A } = await receive('keys')
  const bob = crypto.createDiffieHellman(p, 'hex', g, 'hex')

  const B = bob.generateKeys('hex')
  await send('B', B)

  const s = bob.computeSecret(A, 'hex', 'hex')

  console.log('Vlerat', {
    p, g, A, B, s
  })

  const key = crypto.scryptSync(s, 'salt', 8)
  const { iv, ciphertext } = await receive('ciphertext')
  const decipher = crypto.createDecipheriv('des-cbc', key, Buffer.from(iv, 'hex'))

  let plaintext = decipher.update(ciphertext, 'hex', 'utf8')
  plaintext += decipher.final('utf8')

  console.log('Plaintext:', plaintext)
}
