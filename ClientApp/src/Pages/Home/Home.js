import React from 'react'
import './Home.css';
import Typewriter from 'typewriter-effect'
import { Link } from 'react-router-dom';

const escribirAuto = () => {
   return <Typewriter
           onInit={(typewriter) => {
             typewriter
             .typeString("vida.")
             .pauseFor(3000)
             .deleteAll()
             .typeString("salud.")
             .deleteAll()
             .start();
           }}
           />;
}

export default function Home() {
  return (
    <div className='box'>
      <header>
        <div className='banner'>
          <div className='container-text'>
            <div className='text-flex'>
              <h1 className='text'>Toma Agua.</h1>
              <h1 className='text'>Cambiará tu {escribirAuto()}</h1>
              <p className='description'>Crea un hábito positivo con nuestra botella.</p>
            </div>
            <div className='btn-container'>
              <Link className='button' to='/' style={{ textDecoration: 'none' }}>Comprar</Link>
            </div>
          </div>
        </div>
      </header>
      <section>
        <p className='info'>¿Ya tienes la botella? Descarga la aplicación para dispositivos móviles.</p>
      </section>
    </div>
  );
}
