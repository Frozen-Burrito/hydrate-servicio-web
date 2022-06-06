import React from "react";
import { Link } from "react-router-dom";

export default function ElementoDrawer({ texto, url, icono, accionFinal }) {

  return (
    <Link to={ url } className='elemento-drawer'>
      <div className="prefijo">
        <span className="material-icons">
          { icono }
        </span>

        <p>{ texto }</p>
      </div>

      <div className="sufijo">
        { accionFinal }
      </div>
    </Link>
  );
}