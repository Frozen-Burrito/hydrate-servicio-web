import React, { useState, useEffect } from "react";

import { loadStripe } from "@stripe/stripe-js";
import { Elements } from "@stripe/react-stripe-js";

import useCookie from "../../utils/useCookie";
import { crearPaymentIntent } from "../../api/api_productos";

import { Drawer, BotonIcono, FormStripeCheckout } from "..";

DrawerCompra.defaultProps = {
  mostrar: false,
  producto: null,
  onCancelarCompra: null,
  lado: "izquierda",
};

const stripePromise = loadStripe(process.env.REACT_APP_STRIPE_TEST_PK);

export default function DrawerCompra(props) {

  const { mostrar, producto, onCancelarCompra, lado } = props;

  const { valor: jwt } = useCookie("jwt");

  const [cantidad, setCantidad] = useState(1);
  const [etapaPago, setEtapaPago] = useState(1);

  const [secretoCliente, setSecretoCliente] = useState("");
  const [orden, setOrden] = useState(null);

  function handleCambioCantidad(e) {

    //TODO: Quitar esto, solo es para pruebas temporales.
    console.log("Stripe test PK: ", process.env.REACT_APP_STRIPE_TEST_PK);

    const nuevaCantidad = e.target.value;
    setCantidad(nuevaCantidad);
  }

  function handleContinuar(e, nuevaEtapa) {
    setEtapaPago(nuevaEtapa);
  }

  useEffect(() => {

    async function generarIntentDePago() {
      
      const { secretoCliente, orden: ordenCreada } = await crearPaymentIntent([{
        idProducto: producto.id,
        cantidad,
      }], jwt);

      console.log(ordenCreada);
      setOrden(ordenCreada);

      if (secretoCliente != null) { 
        setSecretoCliente(secretoCliente);
      } else {
        console.log("Error generando intent de pago");
      }
    }

    if (etapaPago === 2 && secretoCliente.length <= 0) {
      generarIntentDePago();
    }

  }, [etapaPago, jwt, cantidad, producto, secretoCliente]);

  const formularioCantidad = producto != null 
  ? (
    <>
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

      <div className="stack horizontal justify-between gap-1 my-2">

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
    </>
  ) : <p>Ningún producto seleccionado.</p>;

  const aparienciaPago = {
    theme: "stripe",
  };

  const opciones = {
    clientSecret: secretoCliente,
    appearance: aparienciaPago
  }

  const stripeCheckout = secretoCliente && (
    <Elements options={opciones} stripe={stripePromise}>
      <FormStripeCheckout 
        total={orden != null ? orden.montoTotal : 0.0 } 
        onCancelarCompra={onCancelarCompra} 
      />
    </Elements>
  );

  const etapasFormulario = [
    formularioCantidad,
    stripeCheckout,
  ];

  function renderElementosDrawer() {

    const componenteFormulario = etapasFormulario[etapaPago - 1];

    if (producto != null) {
      return (
        <div className="contenido-drawer stack vertical justify-between gap-1 py-2 px-1">
          <BotonIcono 
            icono="clear"
            tipo="texto"
            iconoAlFinal={true}
            onClick={onCancelarCompra}
          />
  
          <h4 className="center-text">{ producto.nombre }</h4>
  
          { componenteFormulario }
        </div>
      );
    } else {
      return <p>Error obteniendo proceso de pago.</p>
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