
import { useState } from "react";
import { UsuarioContext } from "./UsuarioContext";

export const UsuarioProvider = ({ children }) => {

    // Recupera o usuário salvo assim que o Provider é iniciado
    const [usuario, setUsuario] = useState(() => {
        try {
            const usuarioSalvo = localStorage.getItem("usuario");

            return usuarioSalvo
                ? JSON.parse(usuarioSalvo)
                : null;

        } catch (error) {
            console.error("Erro ao recuperar usuário:", error);
            localStorage.removeItem("usuario");
            return null;
        }
    });

    // Salva o usuário no estado e no localStorage
    const salvarUsuario = (dadosUsuario) => {
        setUsuario(dadosUsuario);

        localStorage.setItem(
            "usuario",
            JSON.stringify(dadosUsuario)
        );
    };

    // Realiza o logout
    const sair = () => {
        setUsuario(null);

        localStorage.removeItem("usuario");
        localStorage.removeItem("token");
    };

    return (
        <UsuarioContext.Provider
            value={{
                usuario,
                setUsuario: salvarUsuario,
                sair
            }}
        >
            {children}
        </UsuarioContext.Provider>
    );
};
