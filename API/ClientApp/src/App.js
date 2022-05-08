import './App.css';
import {Switch, Route, BrowserRouter} from 'react-router-dom';
import { RutaProtegida } from './components/RutaProtegida/RutaProtegida';

import {
  Home,
  AboutUs,
  DatosAbiertos,
  Products,
  GuiasUsuario,
  CreacionCuenta,
  InicioSesion,
  AdminRecursos,
  AdminComentarios,
  AdminOrdenes,
  AdminUsuarios
} from './Pages';

function App() {
  return (
    <BrowserRouter>
      <Switch>
        <Route path='/' exact>
          <Home />
        </Route>
        <Route path="/about" component={AboutUs} />
        <Route path="/datos-abiertos" component={DatosAbiertos} />
        <Route path="/productos" component={Products} />
        <Route path="/guias-usuario" component={GuiasUsuario} />
        <Route path="/inicio-sesion" component={InicioSesion} />
        <Route path="/creacion-cuenta" component={CreacionCuenta} />

        <Route path="/admin/comentarios" 
          component={
            () => <RutaProtegida rolRequerido={'MODERADOR_COMENTARIOS'}> <AdminComentarios /> </RutaProtegida>
          } 
        />
        <Route path="/admin/ordenes" 
          component={
            () => <RutaProtegida rolRequerido={'ADMIN_ORDENES'}> <AdminOrdenes /> </RutaProtegida>
          } 
        />
        <Route path="/admin/recursos-informativos" 
          component={
            // () => <RutaProtegida rolRequerido={'ADMIN_RECURSOS_INF'}> <AdminRecursos /> </RutaProtegida>
            () => <AdminRecursos />
          } 
        />
        <Route path="/admin/usuarios" 
          component={
            () => <RutaProtegida rolRequerido={'ADMINISTRADOR'}> <AdminUsuarios /> </RutaProtegida>
          } 
        />
      </Switch>
    </BrowserRouter>
  );
}

export default App;
