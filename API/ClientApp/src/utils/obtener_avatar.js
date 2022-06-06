
export const getIniciales = (nombre) => {
  const partes = nombre.split(" ");

  switch (partes.length) {
      case 0: return "An";
      case 1: return partes[0].charAt(0).toUpperCase();
      default: return `${partes[0].charAt(0).toUpperCase()}${partes[1].charAt(0).toUpperCase()}`;
  }
}

export const stringAColor = (string) => {
  let hash = 0;
  let i;

  for (i = 0; i < string.length; i += 1) {
    hash = string.charCodeAt(i) + ((hash << 5) - hash);
  }
  
  let color = '#';
  
  for (i = 0; i < 3; i += 1) {
    const valor = (hash >> (i * 8)) & 0xff;
    color += `00${valor.toString(16)}`.slice(-2);
  }
  
  return color;
}