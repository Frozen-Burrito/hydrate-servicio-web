import React from 'react';
import useCookie from '../../../utils/useCookie';
import { Drawer } from '../../../components/Drawer/Drawer';
import { ElementoDrawer } from '../../../components/Drawer/ElementoDrawer';
import Layout from '../../../components/Layout/Layout';

export function AdminRecursos () {

  const { eliminarCookie: eliminarToken } = useCookie('jwt');

  const elementoCuenta = (
    <ElementoDrawer
      icono='account_circle'
      texto={'Usuario autenticado'}
      url='/perfil'
      accionFinal={(
        <button onClick={() => {
            
          eliminarToken();
        }}>
          <span className="material-icons">
            logout
          </span>
        </button>
      )}
    />
  );

  const elementosMenu = (
    <>
      <ElementoDrawer
        icono='account_circle'
        texto={'Usuarios'}
        url='/admin/usuarios'
      />
      <ElementoDrawer
        icono='dashboard'
        texto={'Órdenes'}
        url='/admin/ordenes'
      />
      <ElementoDrawer
        icono='forum'
        texto={'Comentarios'}
        url='/admin/comentarios'
      />
      <ElementoDrawer
        icono='auto_stories'
        texto={'Recuros Informativos'}
        url='/admin/recursos-informativos'
      />
    </>
  );

  return (
    <Layout>

      <Drawer
        encabezado='Administrar'
        elementos={elementosMenu}
        elementoFinal={elementoCuenta}
      />

      <div className='panel-contenido'>
        <h3>Recursos Informativos</h3>
      </div>
    </Layout>
  )
}