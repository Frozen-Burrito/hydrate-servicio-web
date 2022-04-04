import React from 'react'
import './AboutUs.css';
import Layout from '../../Components/Layout/Layout';
import Footer from '../../Components/Footer/Footer';

const AboutUs = () => {
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
            <h2 className='title-about'>Nuestro Objetivo</h2>
            <p className='text-about'>
            Lorem ipsum dolor sit amet, consectetur adipiscing elit.
            Donec volutpat ex nec erat posuere, et condimentum erat 
            commodo. Nunc luctus, quam vel vehicula congue, libero 
            metus pretium urna, non tincidunt velit leo sit amet purus. 
            Maecenas et mattis nisl. In dignissim dictum eros eget finibus. 
            Nunc ut eros sem. Donec eu viverra tortor, ac pulvinar velit. 
            Nulla eget nisi justo. Morbi sapien dolor, lacinia a rhoncus at, 
            vestibulum quis augue. Ut aliquet elementum eros, quis luctus ligula 
            gravida in.
            </p>
            <p className='text-about'>
            Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec volutpat 
            ex nec erat posuere, et condimentum erat commodo. Nunc luctus, quam vel 
            vehicula congue, libero metus pretium urna, non tincidunt velit leo sit 
            amet purus. Maecenas et mattis nisl. In dignissim dictum eros eget finibus. 
            Nunc ut eros sem. Donec eu viverra tortor, ac pulvinar velit. Nulla eget nisi 
            justo. Morbi sapien dolor, lacinia a rhoncus at, vestibulum quis augue. Ut 
            aliquet elementum eros, quis luctus ligula gravida in.
            </p>
          </div>
        </section>
        <div className='space'></div>
        <Footer />
    </Layout>
  );
}

export default AboutUs;