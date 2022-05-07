import { useCallback, useState } from "react";
import Cookies from 'js-cookie';

const useCookie = (nombreCookie) => {

  const [valor, setValor] = useState(() => Cookies.get(nombreCookie));

  const actualizarCookie = useCallback((nuevoValor, opciones) => {
    Cookies.set(nombreCookie, nuevoValor, opciones);
    setValor(nuevoValor);
  }, [nombreCookie]);

  const eliminarCookie = useCallback(() => {
    Cookies.remove(nombreCookie);
    setValor(null);
  }, [ nombreCookie ]);

  return [valor, actualizarCookie, eliminarCookie];
};

export default useCookie;