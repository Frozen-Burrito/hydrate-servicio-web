import React, { Component } from 'react';
import './App.css';
import {Switch, Route} from 'react-router-dom';
import Navbar from './components/Navbar/Navbar';
import Home from './Pages/Home/Home';
import AboutUs from './Pages/AboutUs/AboutUs';
import DatosAbiertos from './Pages/DatosAbiertos/DatosAbiertos';
import Products from './Pages/Products/Products';
import GuiasUsuario from './Pages/GuiasUsuario/GuiasUsuario';

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <div>
        <Navbar />
        <Switch>
          <Route path='/' exact component={Home}/>
          <Route path="/about" component={AboutUs} />
          <Route path="/datos-abiertos" component={DatosAbiertos} />
          <Route path="/productos" component={Products} />
          <Route path="/guias-usuario" component={GuiasUsuario} />
        </Switch>
      </div>
    );
  }
}
