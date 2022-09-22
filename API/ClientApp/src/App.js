import React, { useEffect } from "react";
import {Switch, Route, BrowserRouter} from "react-router-dom";

import useCookie from "./utils/useCookie";
import { RutaProtegida } from "./components";

import * as Pages from "./Pages";
import "./App.css";
import { esJwtExpirado } from "./utils/parseJwt";

function App() {

  const { valor: jwt, eliminarCookie: removerJwt } = useCookie("jwt");

  useEffect(() => {

    function revisarSiJwtEsValido(token) {

      if (esJwtExpirado(token)) {
        removerJwt();
      }
    }

    if (jwt != null) {
      revisarSiJwtEsValido(jwt);
    }
    
  }, [ jwt, removerJwt ]);

  return (
    <BrowserRouter>
      <Switch>
        <Route path="/" exact>
          <Pages.Home />
        </Route>
        <Route path="/about" component={Pages.AboutUs} />
        {/* <Route path="/datos-abiertos" component={Pages.DatosAbiertos} /> */}
        <Route path="/productos" component={Pages.Products} />
        <Route path="/guias" component={Pages.PaginaGuiasDeUsuario} />
        <Route path="/comentarios" exact component={Pages.ComentariosPage} />
        <Route path="/comentarios/publicar" exact component={Pages.PublicarComentarioPage} />
        <Route path="/comentarios/publicar/:idComentario" component={Pages.PublicarComentarioPage} />
        <Route path="/comentarios/responder/:idComentario" component={Pages.ResponderComentarioPage} />
        <Route path="/inicio-sesion" component={Pages.InicioSesion} />
        <Route path="/creacion-cuenta" component={Pages.CreacionCuenta} />
        <Route path="/llaves/nueva" component={Pages.PaginaGenerarLlaveAPI} />

        <Route path="/perfil/:idUsuario" exact component={Pages.PaginaPerfil} />

        <Route path="/perfil/:idUsuario/comentarios" 
          component={
            () => <RutaProtegida rolRequerido={"NINGUNO"}> <Pages.PaginaComentariosPerfil /></RutaProtegida>
          } 
        />
        <Route path="/perfil/:idUsuario/tablero" 
          component={
            () => <RutaProtegida rolRequerido={"NINGUNO"}> <Pages.PaginaTableroPerfil /></RutaProtegida>
          } 
        />
        <Route path="/perfil/:idUsuario/ordenes" 
          component={
            () => <RutaProtegida rolRequerido={"NINGUNO"}> <Pages.PaginaOrdenesUsuario /></RutaProtegida>
          } 
        />
        <Route path="/perfil/:idUsuario/llaves" 
          component={
            () => <RutaProtegida rolRequerido={"NINGUNO"}> <Pages.PaginaLlavesUsuario /></RutaProtegida>
          } 
        />

        <Route path="/datos-abiertos" exact component={Pages.DatosAbiertos} />

        <Route path="/datos-abiertos/calidad" 
          component={
            () => <RutaProtegida rolRequerido={"NINGUNO"}> <Pages.PaginaCalidadDatosAbiertos /></RutaProtegida>
          } 
        />
        <Route path="/datos-abiertos/metas" 
          component={
            () => <RutaProtegida rolRequerido={"NINGUNO"}> <Pages.PaginaMetasDatosAbiertos /></RutaProtegida>
          } 
        />
        <Route path="/datos-abiertos/consumo" 
          component={
            () => <RutaProtegida rolRequerido={"NINGUNO"}> <Pages.PaginaConsumoDatosAbiertos /></RutaProtegida>
          } 
        />

        <Route path="/admin/comentarios" 
          component={
            () => <RutaProtegida rolRequerido={"MODERADOR_COMENTARIOS"}> <Pages.PaginaAdminComentarios /> </RutaProtegida>
          } 
        />
        <Route path="/admin/ordenes" 
          component={
            () => <RutaProtegida rolRequerido={"ADMIN_ORDENES"}> <Pages.PaginaAdminOrdenes /> </RutaProtegida>
          } 
        />
        <Route path="/admin/recursos-informativos" 
          component={
            () => <RutaProtegida rolRequerido={"ADMIN_RECURSOS_INF"}> <Pages.PaginaAdminRecursos /> </RutaProtegida>
          } 
        />
        <Route path="/admin/usuarios" 
          component={
            () => <RutaProtegida rolRequerido={"ADMINISTRADOR"}> <Pages.PaginaAdminUsuarios /> </RutaProtegida>
          } 
        />
        <Route path="/admin/llaves-api" 
          component={
            () => <RutaProtegida rolRequerido={"ADMINISTRADOR"}> <Pages.PaginaAdminLlaves /> </RutaProtegida>
          } 
        />

        <Route path="*" component={ Pages.Pagina404 } />
      </Switch>
    </BrowserRouter>
  );
}

export default App;
