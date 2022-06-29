import React from 'react';

import './tarjeta.css';

Tarjeta.defaultProps = {
  titulo: "",
  subtitulo: "",
  prefijo: null,
  sufijo: null,
  children: null,
  acciones: null,
  mediaUrl: null,
  alto: null,
  anchoMaximo: "100%",
  elevacion: 1,
  bordeRedondeado: true,
  accionPrincipal: null,
};

export function onClickAccion(e, accion) {
  e.stopPropagation();
  e.preventDefault();

  accion();
}

export default function Tarjeta(props) {

  const { 
    titulo, 
    subtitulo, 
    prefijo, 
    sufijo,
    children, 
    acciones, 
    elevacion, 
    mediaUrl,
    alto,
    anchoMaximo,
    bordeRedondeado,
    accionPrincipal 
  } = props;

  const claseElevacion = `elevacion-${elevacion}`;
  const claseBordes = `borde-${bordeRedondeado ? "redondeado" : "cuadrado"}`;
  const claseAccionPrincipal = accionPrincipal != null ? "con-accion" : "";
  
  return (
    <div 
      className={`tarjeta ${claseElevacion} ${claseBordes} ${claseAccionPrincipal}`}
      style={{ maxWidth: anchoMaximo, height: (alto != null ? alto : "auto") }}
      onClick={accionPrincipal}>
      
      <div className="contenedor-img">
        { mediaUrl != null && mediaUrl.length > 0 && (
          <img src={mediaUrl} alt="Imagen del producto" />
        )}
      </div>

      <div className="contenido">
        { (titulo != null || prefijo != null || subtitulo != null || sufijo != null) && (
          <div className="encabezado mb-1">
            <div>
              {/* Mostrar prefijo de tarjeta, si lo hay. */}
              { prefijo != null && (      
                <div className="prefijo">
                  { prefijo }
                </div>
              )}

              <div className="stack vertical justify-start gap-1">
                { titulo != null && <h4 className="titulo">{ titulo }</h4> }

                { subtitulo != null && <h6 className="subtitulo">{ subtitulo }</h6> }
              </div>
            </div>

            { sufijo != null && (      
              <div className="sufijo">
                { sufijo }
              </div>
            )}
          </div>
        )}

        <div className="cuerpo">
          { children }
        </div>
        
        { acciones != null && (
          <div className="acciones mt-3">
            { acciones }
          </div>
        )}
      </div>
    </div>
  );
}
