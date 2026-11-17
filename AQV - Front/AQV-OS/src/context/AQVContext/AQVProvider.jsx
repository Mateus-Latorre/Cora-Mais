import { useState } from "react";
import api from "../../services/Services";
import { AQVContext } from "./AQVContext";

export const AQVProvider = ({ children }) => {
  const [listagemNotificacao, setListagemNotificacao] = useState([]);

  
const getNotificacao = async (idUsuario) => {
    if (!idUsuario) return;

    try {
        console.log("Buscando notificações do usuário:", idUsuario);

        const resposta = await api.get(`/Notificacao/Usuario/${idUsuario}`);

        console.log("RESPOSTA DA API:", resposta.data);

        // Pega somente a lista de notificações
        setListagemNotificacao(resposta.data.itens ?? []);

    } catch (error) {
        console.error(
            "Erro ao buscar notificações:",
            error.response?.data || error.message
        );
    }
};

const marcarComoLidas = async (idUsuario) => {
    if (!idUsuario) return;

    try {
        console.log("Marcando notificações como lidas para o usuário:", idUsuario);
        const resposta = await api.patch(`/Notificacao/Usuario/${idUsuario}/lidas`);
    } catch (error) {
        console.error(
            "Erro ao marcar notificações como lidas:",
            error.response?.data || error.message
        );
    }
};

  return (
    <AQVContext.Provider
      value={{
        listagemNotificacao,
        getNotificacao,
        marcarComoLidas,
      }}
    >
      {children}
    </AQVContext.Provider>
  );
};
