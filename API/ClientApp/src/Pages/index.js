// Este archivo sirve como conveniencia al importar varias paginas a 
// la vez, hace que todas sean accesibles desde un solo import.

// Rutas comunes.
export { Home } from './Home/Home';
export { AboutUs } from './AboutUs/AboutUs';
export { DatosAbiertos } from './DatosAbiertos/DatosAbiertos';
export { Products } from './Products/Products';
export { GuiasUsuario } from './GuiasUsuario/GuiasUsuario';
export { InicioSesion } from './InicioSesion/InicioSesion';
export { CreacionCuenta } from './CreacionCuenta/CreacionCuenta';
export { ComentariosPage } from './comentarios_page';
export { PublicarComentarioPage } from './publicar_comentario_page';
export { ResponderComentarioPage } from './responder_comentario_page';

// Rutas de perfil de usuario.
export { PaginaPerfil } from './Perfil';
export { PaginaTableroPerfil } from './Perfil/pagina_tablero_usuario';
export { PaginaComentariosPerfil } from './Perfil/pagina_comentarios_usuario';

// Rutas de administracion. 
export { AdminUsuarios } from './Admin/General/AdminUsuarios';
export { AdminComentarios } from './Admin/Comentarios/AdminComentarios';
export { AdminOrdenes } from './Admin/Ordenes/AdminOrdenes';
export { AdminRecursos } from './Admin/RecursosInf/AdminRecursos';