import { useParams, useNavigate } from "react-router-dom";
import Menu from "../../components/menu/menu";
import "./Ocorrencias.css";
import { useContext, useEffect, useState } from "react";
import { UsuarioContext } from "../../context/UsuarioContext/UsuarioContext";
import api from "../../services/Services";
import { Alerta } from "../../components/alerta/alerta";

// Aceita tanto uma lista direta quanto uma resposta contendo "itens"
const extrairLista = (dados) => {
  if (Array.isArray(dados)) return dados;
  if (Array.isArray(dados?.itens)) return dados.itens;
  return [];
};

const Ocorrencias = () => {
  const { idOcorrencia } = useParams();
  const navigate = useNavigate();
  const { usuario } = useContext(UsuarioContext);

  const [dadosOcorrencia, setDadosOcorrencia] = useState(null);

  // Dados do encaminhamento
  const [observacao, setObservacao] = useState("");
  const [tipoDestino, setTipoDestino] = useState("");
  const [idDestino, setIdDestino] = useState("");

  // Pesquisa de destinatários
  const [buscaDestino, setBuscaDestino] = useState("");
  const [listaDestinos, setListaDestinos] = useState([]);
  const [mostrarListaDestinos, setMostrarListaDestinos] = useState(false);
  const [carregandoDestinos, setCarregandoDestinos] = useState(false);
  const [carregandoEncaminhamento, setCarregandoEncaminhamento] =
    useState(false);
  const [encaminhamento, setEncaminhamento] = useState(null);
  const [devolutiva, setDevolutiva] = useState("");
  const idUsuarioLogado = usuario?.idUsuario ?? usuario?.id;

  const recebeuEncaminhamento = Boolean(encaminhamento);
  // Menu de seleção do tipo de destino
  const [mostrarOpcoesDestino, setMostrarOpcoesDestino] = useState(false);

  // ==========================================
  // BUSCAR OCORRÊNCIA
  // ==========================================
  const buscarEncaminhamentoRecebido = async () => {
    try {
      const [responseOco, responseEnc] = await Promise.all([
        api.get(`/Ocorrencia/${idOcorrencia}`),
        api.get(`/Encaminhamento/Ocorrencia/${idOcorrencia}`),
      ]);

      const ocorrencia = responseOco.data;
      const lista = extrairLista(responseEnc.data);

      // Carrega os dados da ocorrência
      setDadosOcorrencia(ocorrencia);

      const idLogado = usuario?.idUsuario ?? usuario?.id;

      // Identifica o encaminhamento que pertence ao usuário
      const recebido = lista.find((item) => {
        // Ignora encaminhamentos que já receberam devolutiva
        if (item.dataDevolutiva != null) {
          return false;
        }

        // Destino: Coordenador
        const coordenadorRecebeu =
          usuario?.perfil === "Coordenador" &&
          item.idUsuarioCoordenador != null &&
          String(item.idUsuarioCoordenador) === String(idLogado);

        // Destino: Responsável do aluno
        // Quem responde é o professor que criou a ocorrência
        const professorRecebeu =
          usuario?.perfil === "Professor" &&
          item.idResponsavel != null &&
          ocorrencia.idUsuario != null &&
          String(ocorrencia.idUsuario) === String(idLogado);

        return coordenadorRecebeu || professorRecebeu;
      });

      setEncaminhamento(recebido || null);

      console.log("Ocorrência:", ocorrencia);
      console.log("Encaminhamento recebido:", recebido);
    } catch (error) {
      console.error(
        "Erro ao buscar informações:",
        error.response?.data || error.message,
      );
    }
  };
  const getOcorrenciaId = async () => {
    try {
      const response = await api.get(`/Ocorrencia/${idOcorrencia}`);

      setDadosOcorrencia(response.data);
    } catch (error) {
      console.error(
        "Erro ao buscar ocorrência:",
        error.response?.data || error.message,
      );
    }
  };
  // ==========================================
  // REGISTRAR DEVOLUTIVA
  // ==========================================

  const registrarDevolutiva = async () => {
    const texto = devolutiva.trim();

    if (!texto) {
      await Alerta({
        title: "Atenção!",
        text: "Digite uma devolutiva antes de enviar.",
        icon: "warning",
      });

      return;
    }

    if (!encaminhamento?.idEncaminhamento) {
      await Alerta({
        title: "Erro!",
        text: "Não foi possível identificar o encaminhamento.",
        icon: "error",
      });

      return;
    }

    try {
      // Envia a devolutiva para o backend
      await api.patch(
        `/Encaminhamento/${encaminhamento.idEncaminhamento}/devolver`,
        JSON.stringify(texto),
        {
          headers: {
            "Content-Type": "application/json",
          },
        },
      );

      setDevolutiva("");
      setEncaminhamento(null);

      // Atualiza os dados da ocorrência
      await getOcorrenciaId();

      await Alerta({
        title: "Devolutiva enviada!",
        text: "Sua devolutiva foi registrada com sucesso.",
        icon: "success",
        confirmButtonText: "Entendi",
      });
    } catch (error) {
      console.error(
        "Erro ao registrar devolutiva:",
        error.response?.data || error.message,
      );

      await Alerta({
        title: "Erro ao enviar devolutiva!",
        text:
          error.response?.data || "Não foi possível registrar sua devolutiva.",
        icon: "error",
      });
    }
  };
  // ==========================================
  // BUSCAR RESPONSÁVEIS OU COORDENADORES
  // ==========================================

  const carregarDestinos = async (tipo) => {
    setTipoDestino(tipo);
    setIdDestino("");
    setBuscaDestino("");
    setListaDestinos([]);
    setMostrarOpcoesDestino(false);
    setMostrarListaDestinos(true);

    try {
      setCarregandoDestinos(true);

      let endpoint = "";

      if (tipo === "Responsavel") {
        const idAluno = dadosOcorrencia?.idAluno;

        if (!idAluno) {
          throw new Error(
            "Não foi possível identificar o estudante desta ocorrência.",
          );
        }

        // Busca somente os responsáveis vinculados ao aluno
        endpoint = `/Responsavel/Aluno/${idAluno}`;
      }

      if (tipo === "Coordenador") {
        // Busca somente usuários com perfil Coordenador
        endpoint = "/Usuario/Coordenadores";
      }

      const response = await api.get(endpoint);
      const lista = extrairLista(response.data);

      setListaDestinos(lista);

      console.log("Destinatários encontrados:", lista);
    } catch (error) {
      console.error(
        "Erro ao buscar destinatários:",
        error.response?.data || error.message,
      );

      setListaDestinos([]);
      setMostrarListaDestinos(false);

      await Alerta({
        title: "Erro ao buscar destinatários!",
        text:
          error.message ||
          "Não foi possível carregar a lista de destinatários.",
        icon: "error",
        confirmButtonText: "Entendi",
      });
    } finally {
      setCarregandoDestinos(false);
    }
  };

  // ==========================================
  // FILTRAR DESTINATÁRIOS PELO NOME
  // ==========================================

  const destinosFiltrados = listaDestinos.filter((destino) => {
    return destino.nome
      ?.toLowerCase()
      .includes(buscaDestino.trim().toLowerCase());
  });

  // ==========================================
  // SELECIONAR DESTINATÁRIO
  // ==========================================

  const selecionarDestino = (destino) => {
    const id =
      tipoDestino === "Responsavel" ? destino.idResponsavel : destino.idUsuario;

    if (!id) {
      console.error("ID do destinatário não encontrado:", destino);
      return;
    }

    setIdDestino(id);
    setBuscaDestino(destino.nome);
    setMostrarListaDestinos(false);

    console.log("Destinatário selecionado:", destino);
    console.log("ID selecionado:", id);
  };

  // ==========================================
  // ENCAMINHAR OCORRÊNCIA
  // ==========================================

  const encaminharOcorrencia = async () => {
    if (!tipoDestino || !idDestino) {
      await Alerta({
        title: "Atenção!",
        text: "Selecione um destinatário antes de encaminhar.",
        icon: "warning",
      });

      return;
    }

    try {
      setCarregandoEncaminhamento(true);

      const dadosEnc = {
        tipoDestino: tipoDestino,
        idDestino: idDestino,
        observacao: observacao.trim() || null,
      };

      console.log("Dados do encaminhamento:", dadosEnc);

      const response = await api.post(
        `/Ocorrencia/${idOcorrencia}/encaminhar`,
        dadosEnc,
      );

      console.log("Ocorrência encaminhada com sucesso:", response.data);

      // Atualiza os dados da ocorrência
      await getOcorrenciaId();

      await Alerta({
        title: "Encaminhamento realizado!",
        text: "A ocorrência foi encaminhada com sucesso.",
        icon: "success",
        confirmButtonText: "Entendi",
      });

      // Limpa os campos
      setObservacao("");
      setTipoDestino("");
      setIdDestino("");
      setBuscaDestino("");
      setListaDestinos([]);
      setMostrarListaDestinos(false);
    } catch (error) {
      console.error(
        "Erro ao encaminhar ocorrência:",
        error.response?.data || error.message,
      );

      await Alerta({
        title: "Erro ao encaminhar!",
        text:
          error.response?.data?.erro ||
          "Não foi possível encaminhar a ocorrência.",
        icon: "error",
      });
    } finally {
      setCarregandoEncaminhamento(false);
    }
  };

  // ==========================================
  // EXCLUIR OCORRÊNCIA
  // ==========================================

  const funcExcluirOcorrencia = async () => {
    const result = await Alerta({
      title: "Excluir ocorrência?",
      text: "Tem certeza de que deseja excluir esta ocorrência?",
      icon: "warning",
      showCancelButton: true,
      confirmButtonText: "Sim, excluir",
      cancelButtonText: "Cancelar",
    });

    if (!result.isConfirmed) return;

    try {
      // 1. Busca os encaminhamentos
      const responseEnc = await api.get(
        `/Encaminhamento/Ocorrencia/${idOcorrencia}`,
      );

      const encaminhamentos = extrairLista(responseEnc.data);

      // 2. Exclui os encaminhamentos
      for (const encaminhamento of encaminhamentos) {
        await api.delete(`/Encaminhamento/${encaminhamento.idEncaminhamento}`);
      }

      // 3. Busca as notificações
      const responseNot = await api.get(
        `/Notificacao/Ocorrencia/${idOcorrencia}`,
      );

      const notificacoes = extrairLista(responseNot.data);

      // 4. Exclui as notificações
      for (const notificacao of notificacoes) {
        await api.delete(`/Notificacao/${notificacao.idNotificacao}`);
      }

      // 5. Exclui a ocorrência
      await api.delete(`/Ocorrencia/${idOcorrencia}`);

      await Alerta({
        title: "Ocorrência excluída!",
        text: "A ocorrência foi excluída com sucesso.",
        icon: "success",
        confirmButtonText: "Entendi",
      });

      navigate("/DashBoard");
    } catch (error) {
      console.error(
        "Erro ao excluir ocorrência:",
        error.response?.data || error.message,
      );

      await Alerta({
        title: "Erro ao excluir!",
        text: "Não foi possível excluir a ocorrência. Tente novamente.",
        icon: "error",
        confirmButtonText: "Entendi",
      });
    }
  };

  // ==========================================
  // STATUS DA OCORRÊNCIA
  // ==========================================

  let status = null;
  let prioridade = null;

  if (dadosOcorrencia !== null) {
    switch (dadosOcorrencia.idOstatus) {
      case "ABERTA":
        status = <span className="oco-badge oco-azul">Aberta</span>;
        break;

      case "ANDAMENTO":
        status = <span className="oco-badge oco-laranja">Andamento</span>;
        break;

      case "FINALIZADA":
        status = <span className="oco-badge oco-azul-escuro">Finalizada</span>;
        break;

      case "FEEDBACK":
        status = <span className="oco-badge oco-branco">Feedback</span>;
        break;

      default:
        status = null;
    }

    if (dadosOcorrencia.prioridade === "Rotina") {
      prioridade = <span className="oco-badge oco-verde">Rotina</span>;
    } else if (dadosOcorrencia.prioridade === "Importante") {
      prioridade = <span className="oco-badge oco-amarelo">Importante</span>;
    } else if (dadosOcorrencia.prioridade === "Urgente") {
      prioridade = <span className="oco-badge oco-vermelho">Urgente</span>;
    }
  }

  // ==========================================
  // CARREGAMENTO INICIAL
  // ==========================================
  useEffect(() => {
    if (idOcorrencia && usuario) {
      buscarEncaminhamentoRecebido();
    }
  }, [idOcorrencia, usuario?.idUsuario, usuario?.id, usuario?.perfil]);

  // ==========================================
  // TELA
  // ==========================================

  return (
    <div className="fundo-pagina oco-body">
      <Menu />

      <div className="oco-div">
        <h1 className="oco-h1">Detalhes da ocorrência</h1>

        {/* TÍTULO + BADGES */}

        <div className="oco-card-top">
          <div className="oco-card-head">
            <span className="oco-icone">
              <img src="/identificacao.svg" alt="" className="oco-icone-img" />
              Título da ocorrência
            </span>

            <div className="oco-titulo-valor">{dadosOcorrencia?.titulo}</div>
          </div>

          <div className="oco-badges">
            {prioridade}
            {status}

            {usuario?.perfil === "AQV" && (
              <button className="btn-excluir" onClick={funcExcluirOcorrencia}>
                Excluir
              </button>
            )}
          </div>
        </div>

        {/* ESTUDANTE E TURMA */}

        <div className="oco-row2">
          <div className="oco-box-metade">
            <label>Nome</label>

            <div className="oco-valor">
              {dadosOcorrencia?.idAlunoNavigation?.nome}
            </div>
          </div>

          <div className="oco-box-metade">
            <label>Turma</label>

            <div className="oco-valor">
              {dadosOcorrencia?.idAlunoNavigation?.idTurmaNavigation?.nome}
            </div>
          </div>
        </div>

        {/* DESCRIÇÃO */}

        <div className="oco-card">
          <div className="oco-card-head">
            <span className="oco-icone">
              <img src="/descriçao.svg" alt="" className="oco-icone-img" />
              Descrição da Ocorrência
            </span>
          </div>

          <div className="oco-descricao">{dadosOcorrencia?.descricao}</div>
        </div>

        {/* ENCAMINHAR OCORRÊNCIA */}

        {usuario?.perfil === "AQV" &&
          dadosOcorrencia?.idOstatus === "ABERTA" && (
            <div className="oco-card">
              <div className="oco-card-head">
                <span className="oco-icone">
                  <img src="/Seta.png" alt="" className="oco-icone-img" />
                  Encaminhar Ocorrência
                </span>
              </div>

              <input
                type="text"
                placeholder="Descreva aqui o necessário"
                className="oco-input"
                maxLength={300}
                value={observacao}
                onChange={(e) => setObservacao(e.target.value)}
              />

              <label className="oco-label">
                Selecione o destino da ocorrência *
              </label>

              <div className="oco-encaminhar-row">
                {/* SELEÇÃO DO TIPO DE DESTINO */}

                <div
                  className="oco-select-wrapper"
                  onBlur={(e) => {
                    if (!e.currentTarget.contains(e.relatedTarget)) {
                      setMostrarOpcoesDestino(false);
                    }
                  }}
                  onKeyDown={(e) => {
                    if (e.key === "Escape") {
                      setMostrarOpcoesDestino(false);
                    }
                  }}
                >
                  <button
                    type="button"
                    className="oco-select-destino"
                    aria-haspopup="true"
                    aria-expanded={mostrarOpcoesDestino}
                    onClick={() =>
                      setMostrarOpcoesDestino(!mostrarOpcoesDestino)
                    }
                  >
                    <span>
                      {tipoDestino === "Responsavel"
                        ? "Responsável do aluno"
                        : tipoDestino === "Coordenador"
                          ? "Coordenador"
                          : "Tipo de destino"}
                    </span>

                    <span className="oco-seta-select">
                      {mostrarOpcoesDestino ? "⌃" : "⌄"}
                    </span>
                  </button>

                  {mostrarOpcoesDestino && (
                    <div className="oco-opcoes-destino">
                      <button
                        type="button"
                        className={`oco-opcao-destino ${
                          tipoDestino === "Responsavel" ? "selecionada" : ""
                        }`}
                        onClick={() => carregarDestinos("Responsavel")}
                      >
                        Responsável do aluno
                      </button>

                      <button
                        type="button"
                        className={`oco-opcao-destino ${
                          tipoDestino === "Coordenador" ? "selecionada" : ""
                        }`}
                        onClick={() => carregarDestinos("Coordenador")}
                      >
                        Coordenador
                      </button>
                    </div>
                  )}
                </div>

                {/* BUSCA E LISTA DE DESTINATÁRIOS */}

                <div
                  className="oco-destino-busca-wrapper"
                  onBlur={(e) => {
                    if (!e.currentTarget.contains(e.relatedTarget)) {
                      setMostrarListaDestinos(false);
                    }
                  }}
                >
                  <input
                    type="text"
                    placeholder={
                      carregandoDestinos
                        ? "Carregando..."
                        : tipoDestino === "Coordenador"
                          ? "Buscar coordenador por nome"
                          : tipoDestino === "Responsavel"
                            ? "Buscar responsável por nome"
                            : "Selecione o tipo primeiro"
                    }
                    className="oco-input"
                    value={buscaDestino}
                    onChange={(e) => {
                      setBuscaDestino(e.target.value);
                      setIdDestino("");
                      setMostrarListaDestinos(true);
                    }}
                    onFocus={() => setMostrarListaDestinos(true)}
                    disabled={!tipoDestino || carregandoDestinos}
                    autoComplete="off"
                  />

                  {mostrarListaDestinos && tipoDestino && (
                    <section className="oco-lista-destinos">
                      {carregandoDestinos ? (
                        <p className="oco-destino-vazio">
                          Carregando destinatários...
                        </p>
                      ) : destinosFiltrados.length > 0 ? (
                        destinosFiltrados.map((destino) => {
                          const id =
                            tipoDestino === "Responsavel"
                              ? destino.idResponsavel
                              : destino.idUsuario;

                          return (
                            <button
                              type="button"
                              className="oco-item-destino"
                              key={id}
                              onMouseDown={(e) => e.preventDefault()}
                              onClick={() => selecionarDestino(destino)}
                            >
                              {destino.nome}
                            </button>
                          );
                        })
                      ) : (
                        <p className="oco-destino-vazio">
                          {listaDestinos.length === 0
                            ? "Nenhum destinatário encontrado."
                            : "Nenhum resultado para essa pesquisa."}
                        </p>
                      )}
                    </section>
                  )}
                </div>

                {/* BOTÃO ENCAMINHAR */}

                <button
                  type="button"
                  className="oco-btn-enviar"
                  disabled={
                    !tipoDestino ||
                    !idDestino ||
                    carregandoEncaminhamento ||
                    carregandoDestinos
                  }
                  onClick={encaminharOcorrencia}
                >
                  <p className="txt-btn-enviar">
                    {carregandoEncaminhamento
                      ? "Encaminhando..."
                      : "Encaminhar"}
                  </p>
                </button>
              </div>
            </div>
          )}
        {usuario?.perfil === "AQV" &&
          dadosOcorrencia?.idOstatus !== "ABERTA" && (
            <div className="oco-card">
              <div className="oco-card-head">
                <span className="oco-icone">
                  <img src="/Seta.png" alt="" className="oco-icone-img" />
                  Encaminhar Ocorrência
                </span>
              </div>
              <p className="oco-encaminhar-msg">
                Esta ocorrência já foi encaminhada, seu status é atual é{" "}
                <strong>{dadosOcorrencia?.idOstatus}</strong>.
              </p>
            </div>
          )}
        {recebeuEncaminhamento && encaminhamento && (
          <div className="oco-card">
            <div className="oco-card-head">
              <span className="oco-icone">Orientações da AQV</span>
            </div>

            <div className="oco-descricao">
              {encaminhamento.observacao?.trim()
                ? encaminhamento.observacao
                : "A AQV não adicionou observações neste encaminhamento."}
            </div>
          </div>
        )}
        {recebeuEncaminhamento && (
          <div className="oco-card">
            <h3>Devolutiva da ocorrência</h3>

            <textarea
              className="oco-input"
              placeholder="Digite sua devolutiva..."
              value={devolutiva}
              onChange={(e) => setDevolutiva(e.target.value)}
            />

            <button
              type="button"
              className="oco-btn-devolutiva"
              onClick={registrarDevolutiva}
            >
              Enviar devolutiva
            </button>
          </div>
        )}
      </div>
    </div>
  );
};

export default Ocorrencias;
