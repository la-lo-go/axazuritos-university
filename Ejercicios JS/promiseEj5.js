/* Implementación con promesas */ let doTask = (iterations) =>
  new Promise((resolve, reject) => {
    const numbers = [];
    let i = 0;
    while (i < iterations && (numbers[numbers.length - 1] !== 6)) {
      const number = 1 + Math.floor(Math.random() * 6);
      numbers.push(number);
      if (number === 6) {
        reject({ error: true, message: "Se ha sacado un 6" });
      }
      i++;
    }

    if (numbers[numbers.length - 1] !== 6) {
      resolve({ error: false, value: numbers });
    }
  });

doTask(5)
  .then((result) => {
    console.log(result.value);
  })
  .catch((err) => {
    console.log(err.message);
  });
