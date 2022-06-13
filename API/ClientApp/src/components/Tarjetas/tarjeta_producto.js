import React from "react";

import { Tarjeta, BotonIcono,  } from "../";

export default function TarjetaProducto({ producto, onComprar }) {

  return (
    <Tarjeta
      titulo={producto.nombre}
      mediaUrl={producto.urlImagen.toString()}
      anchoMaximo={"30%"}
      alto={"30rem"}
      acciones={
        <div className="stack horizontal justify-between gap-2">
          <div className="stack horizontal justify-start">
            <span className="material-icons primario">
              attach_money
            </span>
            <h4 className="primario">
              {producto.precioUnitario.toString()}
            </h4>
          </div>

          <BotonIcono
            icono="shopping_cart"
            color="secundario"
            label="Comprar"
            tipo="fill"
            disabled={producto.disponibles <= 0}
            onClick={() => onComprar(producto.id)}
          />
        </div>
      }
    >
      <p>
        {producto.descripcion.length > 100 
          ? producto.descripcion.substring(0, 150).concat("(...)")
          : producto.descripcion
        }
      </p>
    </Tarjeta>
  );
}