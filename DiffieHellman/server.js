// B
const { listen } = require('simple-socket-session')
listen(session)

async function session(send, receive) {
  const emri = await receive('emri')
  console.log('Konektim me', emri)

  const p = 23
  const g = 5

  const b = random(1, p)
  const B = expmod(g, b, p)
  console.log('Vlera B', B)

  const A = await receive('A')
  console.log('Vlera A', A)
  await send('B', B)

  const s = expmod(A, b, p)
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
