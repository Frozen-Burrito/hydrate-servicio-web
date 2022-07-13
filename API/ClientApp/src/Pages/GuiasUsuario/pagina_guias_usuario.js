import React, { useState, useEffect } from "react";

import { 
  Layout, 
  Footer, 
  Toolbar, 
  BreadCrumb, 
  SearchBox,
  Dropdown,
  BotonIcono,
  DrawerGuias,
  ListItem,
  Tarjeta,
} from "../../components";


const guiasIntro = [
  { url: "guias/intro/instalar", label: "Instalación", contenido: ( 
    <>
      <h2>Instalación</h2>

      <p>Lorem ipsum dolor sit amet consectetur adipisicing elit. Animi similique eos autem eligendi blanditiis, quod facilis eaque quos sint voluptas suscipit voluptates pariatur eveniet dolor ab voluptate quia magnam reprehenderit.</p>

      <img src="../images/placeholder.png" alt="Imagen de prueba" />
    </>
  )},
  { url: "guias/intro/preparar", label: "Preparar la Botella", contenido: ( 
    <>
      <h2>Prepara tu Botella</h2>

      <p>Lorem ipsum dolor sit amet consectetur adipisicing elit. Animi similique eos autem eligendi blanditiis, quod facilis eaque quos sint voluptas suscipit voluptates pariatur eveniet dolor ab voluptate quia magnam reprehenderit.</p>

      <img src="../../images/placeholder.png" alt="Imagen de prueba" />
    </>
  )},
  { url: "guias/intro/uso-diario", label: "Usar la botella en el día a día.", contenido: ( 
    <>
      <h2>Usar la Extensión Inteligente en tu Día a Día</h2>

      <p>Lorem ipsum dolor sit amet consectetur adipisicing elit. Animi similique eos autem eligendi blanditiis, quod facilis eaque quos sint voluptas suscipit voluptates pariatur eveniet dolor ab voluptate quia magnam reprehenderit.</p>

      <img src="../../images/placeholder.png" alt="Imagen de prueba" />
    </>
  )},
];

const secciones = [
  { url: null, label: "Comenzar", guias: guiasIntro },
  { url: null, label: "Conectar la Botella", guias: guiasIntro },
  { url: null, label: "Usar la Aplicación", guias: guiasIntro },
  { url: null, label: "Enviar Comentarios", guias: guiasIntro },
  { url: null, label: "Solución de Problemas", guias: guiasIntro },
];

const idiomasSoportados = [
  "Español",
  "Inglés",
];

export function PaginaGuiasDeUsuario () {

  const [drawerEsVisible, setVisibilidadDrawer] = useState(true);
  const ladoDelDrawer = "izquierda";

  const [guiaActual, setGuiaActual] = useState(secciones[0].guias[0]);
  const [resultadosBusqueda, setResultadosBusqueda] = useState([]);
  const [queryDeGuias, setQueryDeGuias] = useState("");

  const [indiceIdioma, setIndiceIdioma] = useState(0);

  function toggleDrawerLateral(visible = null) {
    setVisibilidadDrawer(visible ?? !drawerEsVisible);
  }

  const dropdownIdioma = (
    <Dropdown 
      onColor="superficie"
      boton={(
        <BotonIcono 
          tipo="dropdown"
          color="fondo"
          icono="translate"
          iconoSufijo="arrow_drop_down"
          elevacion={1}
          label={ idiomasSoportados[indiceIdioma] }
        />
      )}
      items={(
        <>
          { idiomasSoportados.map((idioma, i) => (
            <button 
              key={i} 
              className ="elemento-dropdown" 
              onClick={() => cambiarIdioma(i)}
            >
              { idioma }
            </button>
          ))}
        </>
      )}
    />
  );

  function cambiarIdioma(nuevoIdioma) {
    setIndiceIdioma(nuevoIdioma);
  }

  function onNavegarAGuia(seccion) {
    setGuiaActual(seccion);
    setQueryDeGuias("");
  }

  useEffect(() => {

    if (queryDeGuias != null && queryDeGuias.trim().length > 0) {

      console.log(queryDeGuias)

      const guias = [];

      // Incluir todas las guias, de todas las secciones.
      secciones.forEach(seccion => {
        seccion.guias.forEach(guia => {
          guias.push(guia);
        })
      });

      console.log(guias);

      const resultados = guias.filter(guia => {

        return guia.label.toLowerCase().includes(queryDeGuias.toLowerCase());
      });

      console.log(resultados);

      setResultadosBusqueda(resultados);

    } else {
      setResultadosBusqueda([]);
    }

  }, [queryDeGuias]);

  const menuDeGuias = secciones.map((seccion, indice) => (
    <ListItem
      key={indice}
      titulo={seccion.label}
      colapsable={ true }
    >
      { seccion.guias.map((guia, i) => (
        <ListItem
          key={i}
          titulo={guia.label}
          backgroundColor="superficie"
          seleccionado={ guia.url === guiaActual.url }
          colapsable={ false }
          onClick={() => onNavegarAGuia(guia)}
        />
      ))}
    </ListItem>
  ));

  return (
    <Layout>

      <DrawerGuias 
        lado={ladoDelDrawer}
        guias={menuDeGuias}
        mostrar={drawerEsVisible} 
        onToggle={toggleDrawerLateral}
      />  
      
      <div style={{ overflowX: "hidden" }}>
        <div className={`contenido-desplazable ${drawerEsVisible ? "desplazado" : ""} ${ladoDelDrawer}`}>
          <Toolbar
            tieneDrawer={true}
            onToggleMenu={ () => toggleDrawerLateral() }
            startChildren={
              <BreadCrumb
                ruta={ guiaActual.url } 
                nombreRutaBase="Guías"
              />
            }
            endChildren={
              <>
                { dropdownIdioma }

                <SearchBox
                  icono="search" 
                  iconoSufijo="clear"
                  label="Buscar temas, artículos..." 
                  buscarEnOnChange={true}
                  onBusqueda={setQueryDeGuias}
                />
              </>
            }
          />

          <section className="container stack vertical align-start full-page py-5">
            
            <div className="my-3 expandir-x">

              { queryDeGuias.length > 0 
                ? (
                  <>
                    <h2>Resultados de Búsqueda</h2>

                    { resultadosBusqueda.length > 0 
                      ? resultadosBusqueda.map((resultado, i) => (
                        <div className="mt-2">
                          <Tarjeta
                            titulo={resultado.label}
                            subtitulo={resultado.url}
                            accionPrincipal={ () => onNavegarAGuia(resultado) }
                          />
                        </div>
                      ))
                      : <p>No hay resultados que coincidan con tu búsqueda.</p>}
                  </>
                )
                : guiaActual.contenido 
              }
            </div>
          </section>

          <Footer />
        </div>
      </div>
    </Layout>
  )
}
