import './perfil.css';
import React, { useEffect, useState } from "react";
import useCookie from '../../utils/useCookie';
import { Layout, Avatar, DrawerPerfil } from "../../components";
import { getPerfil, updatePerfil, getInformacionPerfil } from '../../api/api_perfil';

export function PaginaPerfil() {

  const [drawerVisible, setDrawerVisible] = useState(false);
  const {valor: jwt} = useCookie('jwt');

  const [idCuentaUsuario] = useState("3fa85f64-5717-4562-b3fc-2c963f66afa6");

  const [nombre, setNombre] = useState("");
  const [apellido, setApellido] = useState("");
  const [fechaNacimiento, setFechaNacimiento] = useState("");
  const [nombreUsuario, setNombreUsuario] = useState("");
  const [email, setEmail] = useState("");
  const [estatura, setEstatura] = useState(0);
  const [peso, setPeso] = useState(0);
  const [sexoUsuario, setSexoUsuario] = useState(0);
  const [condicionMedica, setCondicionMedica] = useState(0);
  const [idPaisDeResidencia, setIdPaisDeResidencia] = useState(1);

  const [ocupacion] = useState(0);
  const [cantidadMonedas] = useState(0);
  const [numModificaciones] = useState(0);
  const [fechaSyncConGoogleFit] = useState(null);
  const [fechaCreacion] = useState(null);
  const [fechaModificacion] = useState(null);
  const [idEntornoSeleccionado] = useState(1);
  const [idsEntornosDesbloqueados] = useState([1]);
  
  useEffect(() => {

    const obtenerPerfil = async() => {

      const res = await getPerfil(jwt);
      const res1 = await getInformacionPerfil(jwt);
      console.log(res);
      console.log(res1);

      if(res.cuerpo !== undefined && res.ok) {
        setNombre(res.cuerpo.nombre);
        setApellido(res.cuerpo.apellido);
        setFechaNacimiento(res.cuerpo.fechaNacimiento);
        setNombreUsuario(res1.cuerpo.nombreUsuario);
        setEmail(res1.cuerpo.email);
        setPeso(res.cuerpo.peso);
        setEstatura(res.cuerpo.estatura);
        setSexoUsuario(res.cuerpo.sexoUsuario);
        setCondicionMedica(res.cuerpo.condicionMedica);
      }
    }
    obtenerPerfil();
  }, [jwt]);

  const actualizarPerfil = async(e) => {

    e.preventDefault();

    const cambiosPerfil = {
      idCuentaUsuario,
      nombre,
      apellido,
      fechaNacimiento,
      sexoUsuario,
      estatura,
      peso,
      ocupacion,
      condicionMedica,
      idPaisDeResidencia,
      cantidadMonedas,
      numModificaciones,
      fechaSyncConGoogleFit,
      fechaCreacion,
      fechaModificacion,
      idEntornoSeleccionado,
      idsEntornosDesbloqueados,
      nombreUsuario,
      email
    }

    updatePerfil(cambiosPerfil, jwt);
    console.log("Datos Actualizados");
  }

  const nombreCompleto = nombre  + " " + apellido;

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
            <button className={`btn btn-primario`} onClick={ actualizarPerfil } >Editar</button>
          </div>
        </div>

        <div style={{ display: "flex", justifyContent: 'center' }}>
          <Avatar alt={nombreCompleto} />
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
              value={ nombre }
              onChange={(e) => setNombre(e.target.value)}
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
              value={nombreUsuario}
              onChange={(e) => setNombreUsuario(e.target.value)}
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
                value={ idPaisDeResidencia }
                onChange={(e) => setIdPaisDeResidencia(e.target.value)}
                />
            </div>
            <div className='campo-icono'>
              <span className='material-icons'>
                height
              </span>
                <input 
                type="number" 
                name="estatura" 
                className="input" 
                placeholder='Estatura'
                value={ estatura }
                onChange={(e) => setEstatura(e.target.value)}
                />
            </div>
            <div className='campo-icono'>
              <span className='material-icons'>
                person
              </span>
                <input 
                type="number" 
                name="peso" 
                className="input" 
                placeholder='Peso'
                value={ peso }
                onChange={(e) => setPeso(e.target.value)}
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
              value={ apellido }
              onChange={(e) => setApellido(e.target.value)}
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
              value={ email }
              onChange={(e) => setEmail(e.target.value)}
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
                placeholder='Fecha de Nacimiento'
                value={ fechaNacimiento }
                onChange={(e) => setFechaNacimiento(e.target.value)}
                />
            </div>
            <div className='campo-icono'>
              <span className='material-icons'>
                person
              </span>
                <input 
                type="number" 
                name="sexoUsuario" 
                className="input" 
                placeholder='Sexo'
                value={ sexoUsuario }
                onChange={(e) => setSexoUsuario(e.target.value)}
                />
            </div>
            <div className='campo-icono'>
              <span className='material-icons'>
                person
              </span>
                <input 
                type="number" 
                name="condicionMedica" 
                className="input" 
                placeholder='Condicion Medica'
                value={ condicionMedica }
                onChange={(e) => setCondicionMedica(e.target.value)}
                />
            </div>
          </div>
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