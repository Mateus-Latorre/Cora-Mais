import { use, useEffect, useState } from "react";
import { UsuarioContext } from "./UsuarioContext";

export const UsuarioProvider = ({children}) => {
    const [usuario, setUsuario] = useState(null)
    useEffect(() => {
         const usuarioLogado = JSON.parse(localStorage.getItem("usuario"))
        setUsuario(usuarioLogado)   
    }, []);
    return (
        <UsuarioContext.Provider value={{ usuario, setUsuario }}>
            {children}
        </UsuarioContext.Provider>
    )        
}