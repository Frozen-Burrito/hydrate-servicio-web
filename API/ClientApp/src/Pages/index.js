// Este archivo sirve como conveniencia al importar varias paginas a 
// la vez, hace que todas sean accesibles desde un solo import.

// Rutas comunes.
export { Home } from './Home/Home';
export { AboutUs } from './AboutUs/AboutUs';
export { DatosAbiertos } from './DatosAbiertos/DatosAbiertos';
export { Products } from './Products/Products';
export { PaginaGuiasDeUsuario } from './GuiasUsuario/pagina_guias_usuario';
export { InicioSesion } from './InicioSesion/InicioSesion';
export { CreacionCuenta } from './CreacionCuenta/CreacionCuenta';
export { ComentariosPage } from './comentarios_page';
export { PublicarComentarioPage } from './publicar_comentario_page';
export { ResponderComentarioPage } from './responder_comentario_page';
export { PaginaGenerarLlaveAPI } from './pagina_generar_llave_api';

// Rutas de perfil de usuario.
export { PaginaPerfil } from './Perfil';
export { PaginaTableroPerfil } from './Perfil/pagina_tablero_usuario';
export { PaginaComentariosPerfil } from './Perfil/pagina_comentarios_usuario';
export { PaginaOrdenesUsuario } from './Perfil/pagina_ordenes_usuario';
export { PaginaLlavesUsuario } from './Perfil/pagina_llaves_usuario';

// Rutas de administracion. 
export { PaginaAdminUsuarios } from './Admin/pagina_admin_usuarios';
export { PaginaAdminComentarios } from './Admin/pagina_admin_comentarios';
export { PaginaAdminOrdenes } from './Admin/pagina_admin_ordenes';
export { PaginaAdminRecursos } from './Admin/pagina_admin_recursos';
export { PaginaAdminLlaves } from "./Admin/pagina_admin_llaves_api";

export { Pagina404 } from "./404";