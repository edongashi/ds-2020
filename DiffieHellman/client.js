// A
const { connect } = require('simple-socket-session')
connect('http://185.67.178.114:3000', session)

async function session(send, receive) {
  await send('emri', 'Filan')

  const p = 23
  const g = 5

  const a = random(1, p)
  const A = expmod(g, a, p)
  console.log('Vlera A', A)

  await send('A', A)
  const B = await receive('B')
  console.log('Vlera B', B)

  const s = expmod(B, a, p)
  console.log('Sekreti s', s)
}

function random(min, max) {
  return Math.floor(Math.random() * (max - min) + min)
}

function expmod(a, b, n) {
  a = a % n
  let result = 1
  let x = a

  while (b > 0) {
    let lsb = b % 2
    b = Math.floor(b / 2)

    if (lsb == 1) {
      result = result * x
      result = result % n
    }

    x = x * x
    x = x % n
  }

  return result
}
