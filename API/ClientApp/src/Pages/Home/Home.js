import React from 'react'
import { Link } from "react-router-dom";
import './Home.css';
import Typewriter  from 'typewriter-effect';

import { Tarjeta, Layout, Footer } from "../../components";
import BotoneCompra from '../../components/Botones/BotonesCompra';

import googlePlayButton from "./appstores_google_play.png";

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
            <h1 className='text'>Toma Agua.</h1>
                <div className='together'>
                  <h1 className='text edition'>Cambiará tu</h1>
                  <h1 className='text'> {escribirAuto()} </h1>
                </div>
                <h3 className='description'>Forma un hábito saludable con la extensión inteligente Hydrate</h3>
            <div className='btn-container'>
                <BotoneCompra />
            </div>
          </div>
        </div>
      </div>
      <section className='info-container'>
        <div className='stack horizontal justify-between align-center' style={{ maxWidth: "80vw"}}>
          <p className='info'>¿Ya tienes la extensión inteligente? Descarga la app Hydrate para dispositivos móviles.</p>

          <Link to="/">
            <img src={googlePlayButton} alt="Download from Google Play" />
          </Link>
        </div>
      </section>
      <section className='home-second-part'>
        <div className='img-definition-box'>
        </div>
        <div className='bottle-definition-box'>
          <div className='bottle-definition-container-up'>
            <h2 className='tittle-definion-up'>Un Dispositivo Para Acompañarte</h2>
            <div className='text-description-container-up'>
              <span className="material-icons">
                view_in_ar
              </span>
              <p className='text-description-up'>
                La extensión inteligente Hydrate facilita medir tu consumo
                de agua durante tus actividades diarias, como trabajar o hacer
                actividad física. 
              </p>
            </div>
            <div className='text-description-container-up'>
              <span className="material-icons">
                phonelink_ring
              </span>
              <p className='text-description-up'>
              Para obtener el máximo provecho de la extensión inteligente, 
              puedes instalar gratuitamente la app Hydrate. La app ayuda  a formar 
              un hábito saludable de consumo de agua, que se adecúe a tus necesidades
              y estilo de vida.
              </p>
            </div>
          </div>
          <div className='bottle-definition-container-down'>
            <div className='bottle-definition-container'>
              <div className="mr-3">
                <Tarjeta> 
                  <p>La app Hydrate tiene soporte dedicado para atletas y personas con condiciones médicas.</p>
                </Tarjeta>
              </div>

              <div className="mr-3">
                <Tarjeta>
                  <p>Usando una cuenta Hydrate, puedes sincronizar tu progreso de hidratación en todos tus dispositivos.</p>
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
