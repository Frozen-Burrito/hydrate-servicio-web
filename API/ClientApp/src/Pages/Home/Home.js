import React from 'react'
import './Home.css';
import Typewriter  from 'typewriter-effect';

import { Tarjeta, Layout, Footer } from "../../components";
import BotoneCompra from '../../components/Botones/BotonesCompra';

const escribirAuto = () => {
   return <Typewriter
           onInit={(typewriter) => {
             typewriter
             .typeString("vida.")
             .pauseFor(3000)
             .deleteAll()
             .typeString("salud.")
             .pauseFor(3000)
             .deleteAll()
             .start();
           }}
           options={{
             loop: true
           }}
           />;
}

export function Home() {

  return (
    <Layout>
      <div className='banner'>
        <div className='color-contrast'>
          <div className='container-text'>
            <div className='text-flex'>
                <h1 className='text'>Toma Agua.</h1>
                <div className='together'>
                  <h1 className='text edition'>Cambiará tu</h1>
                  <h1 className='text'> {escribirAuto()} </h1>
                </div>
                <h3 className='description'>Crea un hábito positivo con nuestra botella.</h3>
            </div>
            <div className='btn-container'>
                <BotoneCompra />
            </div>
          </div>
        </div>
      </div>
      <section className='info-container'>
        <div className='box-info'>
          <p className='info'>¿Ya tienes la botella? Descarga la aplicación para dispositivos móviles.</p>
        </div>
      </section>
      <section className='home-second-part'>
        <div className='img-definition-box'>
        </div>
        <div className='bottle-definition-box'>
          <div className='bottle-definition-container-up'>
            <h2 className='tittle-definion-up'>Una botella para acompañarte</h2>
            <div className='text-description-container-up'>
              <span className="material-icons">
                view_in_ar
              </span>
              <p className='text-description-up'>
              Lorem ipsum dolor sit amet, consectetur adipiscing elit. 
              Morbi ultrices tortor sed felis suscipit, id volutpat est 
              cursus. Morbi eleifend diam efficitur, vehicula dolor nec, 
              lobortis sem. Cras semper blandit augue sit amet sodales.
              </p>
            </div>
            <div className='text-description-container-up'>
              <span className="material-icons">
                phonelink_ring
              </span>
              <p className='text-description-up'>
              Lorem ipsum dolor sit amet, consectetur adipiscing elit. 
              Morbi ultrices tortor sed felis suscipit, id volutpat est 
              cursus. Morbi eleifend diam efficitur, vehicula dolor nec, 
              lobortis sem. Cras semper blandit augue sit amet sodales.
              </p>
            </div>
          </div>
          <div className='bottle-definition-container-down'>
            <div className='bottle-definition-container'>
              <div className="mr-3">
                <Tarjeta> 
                  <p>Maecenas non purus tincidunt, sollicitudin erat ac, feugiat mi.</p>
                </Tarjeta>
              </div>

              <div className="mr-3">
                <Tarjeta>
                  <p>Maecenas non purus tincidunt, sollicitudin erat ac, feugiat mi.</p>
                </Tarjeta>
              </div>

              <Tarjeta>
                <p>Maecenas non purus tincidunt, sollicitudin erat ac, feugiat mi.</p>
              </Tarjeta>
            </div>
          </div>
        </div>
      </section>
      <Footer />
    </Layout>
  );
}
