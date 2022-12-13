import React from 'react'
import './AboutUs.css';
import Layout from '../../components/Layout/Layout';
import Footer from '../../components/Footer/Footer';

export function AboutUs () {
  return (
    <Layout>
        <div className='about-banner'>
          <div className='color-contrast-about'>
            <div className='container-title-banner'>
              <h1 className='title-banner'>Nuestro proyecto</h1>
            </div>
          </div>
        </div>
        <section className='content-container-about'>
          <div className='text-container-about'>
            <h2 className='title-about'>Origenes</h2>
            <p className='text-about'>
            Hydrate surgió un buen día cuando un par de amigos estábamos hablando sobre
            problemas de salud en la familia y uno de los fundadores de Hydrate, Donnet
            Hazael Pitalua Santana, mencionó que su hermana tiene problemas en los
            riñones por tener síndrome nefrótico y fue ahí donde el otro de los fundadores
            de Hydrate, Fernando Mendoza Velasco, se quedó con esa idea en la cabeza, y
            en el transcurso de los días se le ocurrió realizar un módulo que pueda
            administrar el consumo de agua de un usuario, por lo que Fernando le comentó
            este proyecto a su compañero Donnet, y este le encantó y propuso agregar
            algunas funcionalidades. La idea es que cualquier persona pueda utilizar este
            módulo para poder mejorar o mantener la salud de un usuario con ayuda de
            diferentes plataformas para utilizar este producto, como es una aplicación móvil
            y un sitio web.
            </p>
            <h2 className='title-about'>Objetivo</h2>
            <p className='text-about'>
            Producir aplicaciones móviles y herramientas IoT accesibles para apoyar a
            personas que quieren o necesitan cuantificar y conocer su consumo de agua
            diario con la intención de producir un beneficio a su salud.
            </p>
            <h2 className='title-about'>Misión</h2>
            <p className='text-about'>
            Somos una empresa que desarrolla soluciones tecnológicas que ayudan al
            usuario a controlar su hidratación según sus características físicas y su condición
            médica.
            </p>
            <h2 className='title-about'>Visión</h2>
            <p className='text-about'>
            Ser la empresa líder en herramientas IoT innovadoras para mejorar o conservar
            la salud del usuario.
            </p>
            <h2 className='title-about'>Nuestros participantes</h2>
            <p className='text-about'>
              <ul>
                <li>Fernando Mendoza Velasco</li>
                <li>Donnet Hazael Pitalua Santana</li>
              </ul>
            </p>
          </div>
        </section>
        <div className='space'></div>
        <Footer />
    </Layout>
  );
}