import './perfil.css';

import React, { useState } from "react";

import { Layout, Avatar, DrawerPerfil } from "../../components";

export function PaginaPerfil() {

  const [drawerVisible, setDrawerVisible] = useState(false);

  return (
    <Layout>
      <DrawerPerfil 
        lado="izquierda"
        mostrar={drawerVisible} 
        indiceItemActivo={0}
        onToggle={() => setDrawerVisible(!drawerVisible)}
      />

      <section className='contenedor full-page py-5'>
        <div className="stack contenedor-titulo gap-2 my-3" >
          <div style={{ width: '50%', display: 'flex', justifyContent: 'start' }} className='titulo-perfil' >
            <h3 style={{ textAlign: 'center' }}>Perfil del Usuario</h3>
          </div>
          <div style={{ width: '50%', display: 'flex', justifyContent: 'center' }} className='btn-editar' >
            <button className={`btn btn-primario`} >Editar</button>
          </div>
        </div>

        <div style={{ display: "flex", justifyContent: 'center' }}>
          <Avatar alt="Juan Perez" />
        </div>

        <div className='contenedor-perfil' >
          <div className='izquierda-perfil' >
            <div className='campo-icono'>
              <span className='material-icons'>
                person
              </span>
              <input 
              type="text" 
              name="nombre" 
              className="input" 
              placeholder='Nombre' 
              />
            </div>
            <div className='campo-icono'>
              <span className='material-icons'>
                person
              </span>
              <input 
              type="text" 
              name="nombreUsuario" 
              className="input" 
              placeholder='Nombre del usuario' 
              />
            </div>
            <div className='campo-icono'>
              <span className='material-icons'>
                language
              </span>
                <input 
                type="text" 
                name="pais" 
                className="input" 
                placeholder='País' 
                />
            </div>
          </div>
          <div className='derecha-perfil' >
            <div className='campo-icono'>
              <span className='material-icons'>
                person
              </span>
              <input 
              type="text" 
              name="apellido" 
              className="input" 
              placeholder='Apellido' 
              />
            </div>
            <div className='campo-icono'>
              <span className='material-icons'>
                email
              </span>
              <input 
              type="text" 
              name="correo" 
              className="input" 
              placeholder='Correo Electrónico' 
              />
            </div>
            <div className='campo-icono'>
              <span className='material-icons'>
                calendar_month
              </span>
                <input 
                type="text" 
                name="edad" 
                className="input" 
                placeholder='Edad' 
                />
            </div>
          </div>
        </div>
      </section>
    </Layout>
  );
}