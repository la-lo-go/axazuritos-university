function getPrimos(n, i = 2, primos = []) {  
    if(i > n) {
      console.log(primos);
      return;
    }
  
    if(isPrimo(i)) {
      primos.push(i);
    }
  
    getPrimos(n, ++i, primos);
  }
  
  function isPrimo(n, i = 2) {
    if(n === 2) return true;
    if(n % i === 0) return false;
    if(i * i > n) return true;
  
    return isPrimo(n, ++i);
  }
  
  getPrimos(1000);
  