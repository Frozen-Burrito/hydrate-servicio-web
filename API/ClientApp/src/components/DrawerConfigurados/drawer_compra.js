import React, { useState } from "react";
import { useHistory } from "react-router-dom";

import useCookie from "../../utils/useCookie";
import { getIdUsuarioDesdeJwt } from "../../utils/parseJwt";

import { Drawer, BotonIcono } from "..";

DrawerCompra.defaultProps = {
  mostrar: false,
  producto: null,
  onCancelarCompra: null,
  lado: "izquierda",
};

export default function DrawerCompra(props) {

  const { mostrar, producto, onCancelarCompra, lado } = props;

  const { valor: jwt } = useCookie("jwt");

  const [cantidad, setCantidad] = useState(1);
  const [etapaPago, setEtapaPago] = useState(1);

  const history = useHistory();

  const idUsuario = getIdUsuarioDesdeJwt(jwt);

  function handleCambioCantidad(e) {

    const nuevaCantidad = e.target.value;
    setCantidad(nuevaCantidad);
  }

  function handleContinuar(e, nuevaEtapa) {
    setEtapaPago(nuevaEtapa);
  }

  const formularioCantidad = producto != null 
  ? (
    <div className="contenido-drawer stack vertical justify-between gap-2 py-2">
      <BotonIcono 
        icono="clear"
        tipo="texto"
        iconoAlFinal={true}
        onClick={onCancelarCompra}
      />

      <h4 className="center-text">{ producto.nombre }</h4>

      <img src={ producto.urlImagen } alt="Imagen del producto" />

      <p>
        { producto.descripcion.length > 100  
          ? producto.descripcion.substring(0, 150).concat("(...)")
          : producto.descripcion 
        }
      </p>

      <div className="stack horizontal justify-between gap-2 py-2">
        <div className="campo">
          <div className="campo-con-icono compacto">
            <span className="material-icons">
              shopping_basket
            </span>
            <input 
                type="text" 
                name="cantidad" 
                className="input" 
                placeholder="Cantidad: 1"
                value={cantidad}
                onChange={handleCambioCantidad}
            />
          </div>
        </div>

        <h4 className="primario">
          ${ (producto.precioUnitario * cantidad).toFixed(2) }
        </h4>
      </div>

      <div className="stack horizontal justify-between gap-2 py-2">

        <button 
          className={`btn btn-gris`}
          onClick={onCancelarCompra}
        >
          Cancelar
        </button>

        <button 
          className={`btn btn-secundario ${(cantidad < 1 || cantidad > 1000) ? 'btn-desactivado' : ''}`}
          disabled={(cantidad < 1 || cantidad > 1000)} 
          onClick={(e) => handleContinuar(e, 2)}
        >
          Continuar
        </button>
      </div>
    </div>
  ) : <p>No hay datos del producto.</p>;

  function renderElementosDrawer() {
    switch (etapaPago) {
      case 1:
        return formularioCantidad;
      case 2: 
        return formularioCantidad;
    }
  }

  return (
    <Drawer
      colorFondo="superficie"
      lado={lado}
      mostrar={mostrar}
      conFondo
      onCerrar={onCancelarCompra}
      elementos={renderElementosDrawer()}
    />
  );
}