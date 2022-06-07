import React, { useState, useEffect} from 'react';

import { eliminarRecurso, obtenerRecursos } from '../../api/api_recursos';
import useCookie from '../../utils/useCookie';
import { 
  Layout, 
  Drawer, 
  ElementoDrawer, 
  FormAgregarRecurso, 
  TablaRecursosInf 
} from '../../components';

export function PaginaAdminRecursos () {

  const { valor: jwt, eliminarCookie: eliminarToken } = useCookie('jwt');
  
  const [recursosInformativos, setRecursosInformativos] = useState([]);

  const [recursoSel, setRecursoSel] = useState(null);

  const [estaCargando, setEstaCargando] = useState(false);
  const [tieneError, setTieneError] = useState(false);

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

  // Es invocada cuando el usuario realiza un cambio con un recurso,
  // puede ser creación o modificación.
  // Si fue creado, agrega el nuevo recurso a la colección.
  // Si fue modificado, actualiza su valor en la colección.
  function manejarCambioRecurso(recursoModificado) {
    const recursos = recursosInformativos.slice();

    let indiceRecurso = -1;

    for (var i = 0; i < recursos.length; ++i) {
      if (recursos[i].id === recursoModificado.id) {
        indiceRecurso = i;
      }
    }

    if (indiceRecurso < 0) {
      // El recurso fue creado (no existia en la coleccion de recursos).
      recursos.push(recursoModificado);
    } else {
      // El recurso fue editado (ya existia en la coleccion).
      recursos[indiceRecurso] = recursoModificado;
    }

    console.log(recursosInformativos);

    setRecursosInformativos(recursos);
  }

  function manejarEditarRecurso(idRecurso) {

    const recursoAModificar = recursosInformativos.find(r => r.id === idRecurso);

    console.log('Recurso a modificar: ', recursoAModificar);

    setRecursoSel(recursoAModificar);
  }

  async function manejarEliminarRecurso(idRecurso) {
    setEstaCargando(true);

    console.log('Id del recurso a eliminar: ', idRecurso);
    
    const resultado = await eliminarRecurso(jwt, idRecurso);

    if (resultado.ok && resultado.status === 204) {
      // La petición de eliminación del recurso fue exitosa.
      console.log(resultado.cuerpo);

      const recursosActualizados = recursosInformativos.filter(r => r.id !== idRecurso);

      setRecursosInformativos([ ...recursosActualizados ]);

    } else if (resultado.status >= 500) {
      console.log(resultado.cuerpo);

    } else if (resultado.status >= 400) {

      console.log(resultado.cuerpo);
    }

    setEstaCargando(false);
  }

  useEffect(() => {

    async function obtenerDatos() {
      setEstaCargando(true);
      // Obtener todos los recursos informativos.
      const resultado = await obtenerRecursos(jwt);

      if (resultado.ok && resultado.status === 200) {
        // Si la peticion fue exitosa, actualizar la lista
        // de recursos informativos.
        setRecursosInformativos(resultado.cuerpo);

        console.log(resultado.cuerpo);

      } else {
        setTieneError(true);
      }

      setEstaCargando(false);
    }
    
    obtenerDatos();
  }, [jwt]);

  return (
    <Layout>

      <Drawer
        encabezado='Administrar'
        elementos={elementosMenu}
        elementoFinal={elementoCuenta}
      />

      <div className='panel-contenido'>
        <h3>Recursos Informativos</h3>

        <FormAgregarRecurso 
          recursoActual={recursoSel} 
          onRecursoModificado={manejarCambioRecurso}
        />

        <TablaRecursosInf
          recursosInformativos={recursosInformativos}
          onEditarRecurso={manejarEditarRecurso}
          onEliminarRecurso={manejarEliminarRecurso}
          estaCargando={estaCargando}
          tieneError={tieneError}
        />
      </div>
    </Layout>
  )
}