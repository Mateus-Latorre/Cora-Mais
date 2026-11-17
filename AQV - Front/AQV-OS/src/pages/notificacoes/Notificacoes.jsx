import { useContext, useEffect, useState } from "react";
import Menu from "../../components/menu/menu";
import "./Notificacoes.css";

import { AQVContext } from "../../context/AQVContext/AQVContext";
import { UsuarioContext } from "../../context/UsuarioContext/UsuarioContext";
import api from "../../services/Services";

const Notificacoes = () => {
  // Estados
  const [aba, setAba] = useState("todas");
  const [usuariosPorNotificacao, setUsuariosPorNotificacao] = useState({});

  // Contextos
  const { listagemNotificacao, getNotificacao, marcarComoLidas } = useContext(AQVContext);
  const { usuario } = useContext(UsuarioContext);

  // Garante que notificacoes seja sempre um array
  const notificacoes = Array.isArray(listagemNotificacao)
    ? listagemNotificacao
    : [];

  // ID do usuário logado
  const idLogado = usuario?.idUsuario ?? usuario?.id;

  // 1. Busca as notificações do usuário logado
  useEffect(() => {
    if (idLogado) {
      getNotificacao(idLogado);
    }
  }, [idLogado]);
  useEffect(() => {
    if (idLogado) {
      marcarComoLidas(idLogado);
    }
  }, [idLogado]);
  // 2. Busca a ocorrência e depois o usuário que registrou
  useEffect(() => {
    let ativo = true;

    const carregarUsuarios = async () => {
      const cacheUsuarios = new Map();

      const resultados = await Promise.all(
        notificacoes.map(async (notificacao) => {
          try {
            // Verifica se a notificação possui uma ocorrência
            if (!notificacao.idOcorrencia) {
              return [
                notificacao.idNotificacao,
                { usuario: null, carregado: true },
              ];
            }

            // PRIMEIRO: busca a ocorrência relacionada à notificação
            const respostaOcorrencia = await api.get(
              `/Ocorrencia/${notificacao.idOcorrencia}`,
            );

            console.log("DADOS DA OCORRÊNCIA:", respostaOcorrencia.data);

            // Pega o ID do usuário que registrou a ocorrência
            const idUsuario = respostaOcorrencia.data?.idUsuario;

            console.log("ID DO USUÁRIO DA OCORRÊNCIA:", idUsuario);

            if (!idUsuario) {
              return [
                notificacao.idNotificacao,
                { usuario: null, carregado: true },
              ];
            }

            // SEGUNDO: busca os dados do usuário
            // Evita consultar várias vezes o mesmo usuário
            if (!cacheUsuarios.has(idUsuario)) {
              cacheUsuarios.set(
                idUsuario,

                api
                  .get(`/Usuario/${idUsuario}`)
                  .then((resposta) => resposta.data)
                  .catch((error) => {
                    console.error(
                      "Erro ao buscar usuário:",
                      error.response?.data || error.message,
                    );

                    return null;
                  }),
              );
            }

            const dadosUsuario = await cacheUsuarios.get(idUsuario);

            console.log("DADOS DO USUÁRIO:", dadosUsuario);

            // Associa o usuário à notificação correspondente
            return [
              notificacao.idNotificacao,
              {
                usuario: dadosUsuario,
                carregado: true,
              },
            ];
          } catch (error) {
            console.error(
              "Erro ao buscar ocorrência ou usuário:",
              error.response?.data || error.message,
            );

            return [
              notificacao.idNotificacao,
              { usuario: null, carregado: true },
            ];
          }
        }),
      );

      if (ativo) {
        setUsuariosPorNotificacao(Object.fromEntries(resultados));
      }
    };

    if (notificacoes.length > 0) {
      carregarUsuarios();
    } else {
      setUsuariosPorNotificacao({});
    }

    return () => {
      ativo = false;
    };
  }, [notificacoes]);

  // 3. Filtra as notificações não lidas
  const notificacoesNaoLidas = notificacoes.filter(
    (notificacao) => !notificacao.lida,
  );

  const notificacoesFiltradas =
    aba === "todas" ? notificacoes : notificacoesNaoLidas;

  // 4. Tela
  return (
    <div className="body">
      {/* Menu */}
      <Menu />

      <div className="container">
        {/* Cabeçalho */}
        <div className="cabecalho">
          <div className="titulo">
            <h1 className="titulo_l">Notificações</h1>

            <p className="subtitulo">
              Acompanhe as ocorrências registradas pela equipe.
            </p>
          </div>

          <div className="btn-lidas">
            <button className="noti_lidas" onClick={() => marcarComoLidas(idLogado)}>
              <img src="/lidas.png" alt="" className="lidas_img" />

              <p className="noti-txt">Marcar todas como lidas</p>
            </button>
          </div>
        </div>

        {/* Filtros */}
        <div className="filtro" role="tablist">
          <button
            role="tab"
            aria-selected={aba === "todas"}
            className={aba === "todas" ? "ativo" : ""}
            onClick={() => setAba("todas")}
          >
            Todas <span>{notificacoes.length}</span>
          </button>

          <button
            role="tab"
            aria-selected={aba === "naoLidas"}
            className={aba === "naoLidas" ? "ativo" : ""}
            onClick={() => setAba("naoLidas")}
          >
            Não lidas <span>{notificacoesNaoLidas.length}</span>
          </button>
        </div>

        {/* Lista de notificações */}
        <div className="notis">
          {notificacoesFiltradas.length === 0 ? (
            <p className="noti-txt">Nenhuma notificação encontrada.</p>
          ) : (
            notificacoesFiltradas.map((notificacao) => {
              // Formatação da data
              const dataObjeto = notificacao.dataEnvio
                ? new Date(notificacao.dataEnvio)
                : null;

              const dataValida = dataObjeto && !isNaN(dataObjeto.getTime());

              const horaFormatada = dataValida
                ? dataObjeto.toLocaleTimeString("pt-BR", {
                    hour: "2-digit",
                    minute: "2-digit",
                  })
                : "Hora não disponível";

              const dataFormatada = dataValida
                ? dataObjeto.toLocaleDateString("pt-BR")
                : "Data não disponível";

              const ehHoje =
                dataValida &&
                dataObjeto.toDateString() === new Date().toDateString();

              const nomeData = ehHoje ? "Hoje" : dataFormatada;

              // Recupera o usuário associado à ocorrência
              const resultadoUsuario =
                usuariosPorNotificacao[notificacao.idNotificacao];

              const dadosUsuario = resultadoUsuario?.usuario;

              const nomeUsuario = !resultadoUsuario
                ? "Buscando autor..."
                : (dadosUsuario?.nome ?? "Usuário não identificado");

              return (
                <div
                  className="notificacao-item"
                  key={notificacao.idNotificacao}
                >
                  <p className="noti-txt">{nomeData}</p>

                  <div className="campo-todas-notis">
                    <div className="campo-noti">
                      {/* Ícone */}
                      <div className="campo-noti-img">
                        <img src="/sino.png" alt="" className="img-noti" />
                      </div>

                      {/* Informações */}
                      <div className="campo-noti-txt">
                        <p className="txt-noti">
                          <span className="noti-prof">{nomeUsuario}</span>{" "}
                          {notificacao.mensagem}
                        </p>

                        <p className="hora-txt">{horaFormatada}</p>
                      </div>
                    </div>
                  </div>
                </div>
              );
            })
          )}
        </div>
      </div>
    </div>
  );
};

export default Notificacoes;
