let libro = {
    titulo: "El Quijote",
    paginas: 800,
    precio: 7.5,
    peso: 200,
}

// duplica todas las propiedades del objeto libro
let libro2 = {...libro};
delete libro2.peso;
for (let prop in libro2) {
    if(typeof libro2[prop] == 'number') {
        libro2[prop] = libro2[prop] * 2;
    }
}

console.log(libro2);