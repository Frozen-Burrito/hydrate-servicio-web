import './App.css';
import {Switch, Route} from 'react-router-dom';
import Home from './Pages/Home/Home';
import AboutUs from './Pages/AboutUs/AboutUs';
import DatosAbiertos from './Pages/DatosAbiertos/DatosAbiertos';
import Products from './Pages/Products/Products';
import GuiasUsuario from './Pages/GuiasUsuario/GuiasUsuario';
import InicioSesion from './Pages/InicioSesion/InicioSesion';
import CreacionCuenta from './Pages/CreacionCuenta/CreacionCuenta';
import {BrowserRouter} from 'react-router-dom';

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
      </Switch>
    </BrowserRouter>
  );
}

export default App;
