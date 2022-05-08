
export class ErrorDeValidacion {
  static ninguno = new ErrorDeValidacion('ninguno');
  static correoVacio = new ErrorDeValidacion('correoVacio');
  static correoNoValido = new ErrorDeValidacion('correoNoValido');
  static nombreUsuarioVacio = new ErrorDeValidacion('nombreUsuarioVacio');
  static nombreUsuarioMuyCorto = new ErrorDeValidacion('nombreUsuarioMuyCorto');
  static nombreUsuarioMuyLargo = new ErrorDeValidacion('nombreUsuarioMuyLargo');
  static nombreUsuarioNoValido = new ErrorDeValidacion('nombreUsuarioNoValido');
  static passwordVacio = new ErrorDeValidacion('passwordVacio');
  static passwordMuyCorto = new ErrorDeValidacion('passwordMuyCorto');
  static passwordMuyLargo = new ErrorDeValidacion('passwordMuyLargo');
  static passwordNoValido = new ErrorDeValidacion('passwordNoValido');
  static passConfNoCoincide = new ErrorDeValidacion('passConfNoCoincide');

  constructor(error) {
    this.error = error;
  }

  toString() {
    return `ErrorDeValidacion.${this.error}`;
  }
}

export class ErrorDeAutenticacion {
  static ninguno = new ErrorDeAutenticacion('ninguno');
  static errCredenciales = new ErrorDeAutenticacion('errCredenciales');
  static usuarioExiste = new ErrorDeAutenticacion('usuarioExiste');
  static usuarioNoExiste = new ErrorDeAutenticacion('usuarioNoExiste');
  static passwordIncorrecto = new ErrorDeAutenticacion('passwordIncorrecto');
  static formatoIncorrecto = new ErrorDeAutenticacion('formatoIncorrecto');
  static servicioNoDisponible = new ErrorDeAutenticacion('servicioNoDisponible');

  constructor(error) {
    this.error = error;
  }

  valores() {
    return Object.keys(this);
  }

  toString() {
    return `ErrorDeAutenticacion.${this.error}`;
  }
};

export class ErrorDeRecurso {
  static ninguno = new ErrorDeRecurso('ninguno');
  static errTituloVacio = new ErrorDeRecurso('errTituloVacio');
  static errTituloMuyLargo = new ErrorDeRecurso('errTituloMuyLargo');
  static errUrlVacia = new ErrorDeRecurso('errUrlVacia');
  static errUrlSinHttps = new ErrorDeRecurso('errUrlSinHttps');
  static errFechaNoValida = new ErrorDeRecurso('errFechaNoValida');
  static errDescripcionMuyLarga = new ErrorDeRecurso('errDescripcionMuyLarga');

  constructor(error) {
    this.error = error;
  }

  valores() {
    return Object.keys(this);
  }

  toString() {
    return `ErrorDeRecurso.${this.error}`;
  }
};

const regexCorreo = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

const regexNombreUsuario = /^[a-z0-9_-]{4,20}$/;

const regexPassword = /^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$/;

const esCorreoValido = (correo) => regexCorreo.test(correo);

export const estaVacio = (string) => string === null || string === '';

// Retorna null si el correo introducido es valido. Produce correoVacio
// cuando el correo es null o un string vacio, y correoNoValido si el 
// correo no es valido.
export const validarCorreo = (correo) => {
  if (correo === null || correo === '') return ErrorDeValidacion.correoVacio;

  return esCorreoValido(correo) ? null : ErrorDeValidacion.correoNoValido;
}

export const validarNombreUsuario = (nombreUsuario) => {
  if (estaVacio(nombreUsuario)) return ErrorDeValidacion.nombreUsuarioVacio;

  if (nombreUsuario.length < 4) return ErrorDeValidacion.nombreUsuarioMuyCorto;

  if (nombreUsuario.length > 20) return ErrorDeValidacion.nombreUsuarioMuyLargo;

  return regexNombreUsuario.test(nombreUsuario) ? null : ErrorDeValidacion.nombreUsuarioNoValido;
}

export const validarPassword = (password) => {
  if (estaVacio(password)) return ErrorDeValidacion.passwordVacio;

  if (password.length < 8) return ErrorDeValidacion.passwordMuyCorto;

  if (password.length > 40) return ErrorDeValidacion.passwordMuyLargo;

  return regexPassword.test(password) ? null : ErrorDeValidacion.passwordNoValido;
}

export const validarConfPassword = (password, confPassword) => {
  if (!estaVacio(password) && password !== confPassword) {
    return ErrorDeValidacion.passConfNoCoincide;
  } else {
    return null;
  }
}

export const validarTituloRecurso = (titulo) => {
  if (estaVacio(titulo)) return ErrorDeRecurso.errTituloVacio;

  if (titulo.length > 80) return ErrorDeRecurso.errTituloMuyLargo;

  return null;
}

export const validarFechaPub = (fechaPub) => {
  if (fechaPub > new Date()) {
    return ErrorDeRecurso.errFechaNoValida;
  } else {
    return null;
  }
}

export const validarUrl = (url) => {
  if (estaVacio(url)) {
    return ErrorDeRecurso.errUrlVacia;
  } else if (!url.startsWith("https")) {
    return ErrorDeRecurso.errUrlSinHttps;
  } else {
    return null;
  }
}

export const validarDescripcionRecurso = (descripcion) => {
  if (!estaVacio(descripcion) && descripcion.lenth > 500) {
    return ErrorDeRecurso.errDescripcionMuyLarga;
  } else {
    return null;
  }
}