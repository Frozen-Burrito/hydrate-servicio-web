import './App.css';
import {Switch, Route, BrowserRouter} from 'react-router-dom';
import { RutaProtegida } from './components/RutaProtegida/RutaProtegida';

import * as Pages from './Pages';

function App() {
  return (
    <BrowserRouter>
      <Switch>
        <Route path='/' exact>
          <Pages.Home />
        </Route>
        <Route path="/about" component={Pages.AboutUs} />
        <Route path="/datos-abiertos" component={Pages.DatosAbiertos} />
        <Route path="/productos" component={Pages.Products} />
        <Route path="/guias-usuario" component={Pages.GuiasUsuario} />
        <Route path="/comentarios" exact component={Pages.ComentariosPage} />
        <Route path="/comentarios/publicar" exact component={Pages.PublicarComentarioPage} />
        <Route path="/comentarios/publicar/:idComentario" component={Pages.PublicarComentarioPage} />
        <Route path="/comentarios/responder/:idComentario" component={Pages.ResponderComentarioPage} />
        <Route path="/inicio-sesion" component={Pages.InicioSesion} />
        <Route path="/creacion-cuenta" component={Pages.CreacionCuenta} />

        <Route path="/perfil/:idUsuario" exact component={Pages.PaginaPerfil} />
        <Route path="/perfil/:idUsuario/comentarios" component={Pages.PaginaComentariosPerfil} />
        <Route path="/perfil/:idUsuario/tablero" component={Pages.PaginaTableroPerfil} />

        <Route path="/admin/comentarios" 
          component={
            () => <RutaProtegida rolRequerido={'MODERADOR_COMENTARIOS'}> <Pages.AdminComentarios /> </RutaProtegida>
          } 
        />
        <Route path="/admin/ordenes" 
          component={
            () => <RutaProtegida rolRequerido={'ADMIN_ORDENES'}> <Pages.AdminOrdenes /> </RutaProtegida>
          } 
        />
        <Route path="/admin/recursos-informativos" 
          component={
            () => <RutaProtegida rolRequerido={'ADMIN_RECURSOS_INF'}> <Pages.AdminRecursos /> </RutaProtegida>
          } 
        />
        <Route path="/admin/usuarios" 
          component={
            () => <RutaProtegida rolRequerido={'ADMINISTRADOR'}> <Pages.AdminUsuarios /> </RutaProtegida>
          } 
        />
      </Switch>
    </BrowserRouter>
  );
}

export default App;
