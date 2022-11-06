import React, { useState, useEffect } from "react";

import { 
  PaymentElement,
  useStripe,
  useElements,
} from "@stripe/react-stripe-js";

import { getUsernameYCorreo } from "../../api/api_usuarios";
import useCookie from "../../utils/useCookie";
import { getIdUsuarioDesdeJwt } from "../../utils/parseJwt";

export default function FormStripeCheckout(props) {

  const { total, onCancelarCompra } = props;

  const stripe = useStripe();
  const elementos = useElements();

  const [idUsuario, setIdUsuario] = useState(null);
  const [emailUsuario, setEmailUsuario] = useState(null);

  const [debeEnviarEmail, setDebeEnviarEmail] = useState(false);

  const [mensaje, setMensaje] = useState(null);
  const [estaCargando, setEstaCargando] = useState(false);

  const { valor: jwt } = useCookie("jwt");

  async function getEmailDeUsuario() {

    try {
      const { email } = await getUsernameYCorreo(jwt);

      console.log("Email del usuario: ", email);

      setEmailUsuario(email);
    } catch (e) {
      setMensaje("No fue posible obtener tu correo.");
    }
  }

  useEffect(() => {

    if (!stripe) {
      return;
    }

    const secretoCliente = new URLSearchParams(window.location.search).get(
      "payment_intent_client_secret"
    );
  
    if (!secretoCliente) {
      return;
    }

    async function getIntentDePago() {

      setEstaCargando(true);

      const intentDePago = await stripe.retrievePaymentIntent(secretoCliente);

      setEstaCargando(false);

      switch(intentDePago.status) {
        case "succeeded":
          setMensaje("Pago exitoso!");
          break;
        case "processing":
          setMensaje("Procesando tu pago.");
          break;
        case "requires_payment_method":
          setMensaje("Tu pago no fue exitoso, por favor intenta de nuevo.");
          break;
        default:
          setMensaje("Algo salió mal.");
          break;
      }
    }

    getIntentDePago();

    if (jwt != null) {
      const id = getIdUsuarioDesdeJwt(jwt);

      setIdUsuario(id);
    }

  }, [stripe, jwt]);
  
  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!stripe || !elementos) {
      // Stripe.js no ha cargado todavia.
      // Desactivar el formulario hasta que Stripe.js haya cargado.
      return;
    }

    setEstaCargando(true);

    let confirmParams = {
      // La URL de la página de pago completado.
      return_url: `https://servicio-web-hydrate.azurewebsites.net/perfil/${idUsuario}`,
    };

    // Si el usuario lo desea, enviarle un correo de confirmacion.
    if (debeEnviarEmail) confirmParams.receipt_email = emailUsuario;

    console.log(confirmParams);

    const { error } = await stripe.confirmPayment({
      elements: elementos,
      confirmParams
    });

    // Esta parte del código solo se ejecuta cuando confirmPayment()
    // produce un error inmediato de pago.
    if (error.type === "card_error" || error.type === "validation_error") {
      setMensaje(error.message);
    } else {
      setMensaje("Ocurrió un error inesperado.");
    }

    setEstaCargando(false);
  }

  return (
    <> 
      <h5 className="subtitulo-drawer center-text mb-2">Información de Pago</h5>

      <form id="payment-form" onSubmit={handleSubmit}>
        <PaymentElement id="payment-element" />

        <div className="stack horizontal gap-2 justify-start mt-1">
          <input 
            type="checkbox" 
            checked={debeEnviarEmail} 
            onChange={() => {
              setDebeEnviarEmail(!debeEnviarEmail);

              if (!debeEnviarEmail) {
                getEmailDeUsuario();
              }
            }} 
          />

          <p>Enviar un correo con el comprobante de pago.</p>
        </div>

        <div className="stack horizontal justify-end gap-2 py-2">
          <div className="end-text">
            <h6>Total</h6>

            <h4 className="primario">
              ${ total.toFixed(2) }
            </h4>
          </div>
        </div>

        { mensaje && <div id="payment-message">{ mensaje }</div>}

        <div className="stack horizontal justify-between gap-1 my-2">
          <button 
            className={`btn btn-gris`}
            onClick={onCancelarCompra}
          >
            Cancelar
          </button>

          <button 
            id="submit"
            className={`btn btn-secundario`}
            disabled={estaCargando || !stripe || !elementos} 
          >
            <span id="button-text">
              { estaCargando ? <div className="spinner" id="spinner"></div> : "Pagar" }
            </span>
          </button>
        </div>
      </form>
    </>
  );
}