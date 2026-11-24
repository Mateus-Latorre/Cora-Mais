
import { useParams } from "react-router-dom";
import Menu from "../../components/menu/menu";
import "./Ocorrencias.css";
import { useContext, useEffect, useState } from "react";
import { UsuarioContext } from "../../context/UsuarioContext/UsuarioContext";
import api from "../../services/Services";

const Ocorrencias = () => {
    const { idOcorrencia } = useParams();
    const { ususario } = useContext(UsuarioContext);

    const [dadosOcorrencia, setDadosOcorrencia] = useState(null);
    const [observacao, setObservacao] = useState("");
    const [tipoDestino, setTipoDestino] = useState("");
    const [idDestino, setIdDestino] = useState("");
    const [buscaDestino, setBuscaDestino] = useState("");

    // Controla a abertura do menu personalizado
    const [mostrarOpcoesDestino, setMostrarOpcoesDestino] = useState(false);

    const dadosEnc = {
        tipoDestino: tipoDestino,
        idDestino: idDestino,
        observacao: observacao,
    };

    const encaminharOcorrencia = async (dadosEnc) => {
        try {
            const response = await api.post(`/Ocorrencia/${idOcorrencia}/encaminhar`,
                dadosEnc
            );
            console.log("Ocorrência encaminhada com sucesso:", response.data);
        } catch (error) {
            console.error(
                "Erro ao encaminhar ocorrência:",
                error.response?.data || error.message
            );
        }
    };

    const getOcorrenciaId = async (idOcorrencia) => {
        try {
            const response = await api.get(`/Ocorrencia/${idOcorrencia}`);
            setDadosOcorrencia(response.data);
        } catch (error) {
            console.error(
                "Erro ao buscar ocorrência:",
                error.response?.data || error.message
            );
        }
    };

    let status = null;

    if (dadosOcorrencia !== null) {
        if (dadosOcorrencia.idOstatus == "ABERTA") {
            status = <span className="oco-badge oco-azul">Aberta</span>;
        } else if (dadosOcorrencia.idOstatus == "ANDAMENTO") {
            status = <span className="oco-badge oco-laranja">Andamento</span>;
        } else if (dadosOcorrencia.idOstatus == "FINALIZADA") {
            status = <span className="oco-badge oco-azul-escuro">Finalizada</span>;
        } else if (dadosOcorrencia.idOstatus == "FEEDBACK") {
            status = <span className="oco-badge oco-branco">Feedback</span>;
        }
    }

    useEffect(() => {
        if (idOcorrencia) {
            getOcorrenciaId(idOcorrencia);
        }
    }, [idOcorrencia]);

    return (
        <div className="oco-body">
            <Menu />

            <div className="oco-div">
                <h1 className="oco-h1">Detalhes da ocorrência</h1>

                {/* **************** TÍTULO + BADGES **************** */}

                <div className="oco-card-top">
                    <div className="oco-card-head">
                        <span className="oco-icone">
                            <img
                                src="../../../../public/identificacao.svg"
                                alt=""
                                className="oco-icone-img"
                            />
                            Título da ocorrência
                        </span>

                        <div className="oco-titulo-valor">
                            {dadosOcorrencia?.titulo}
                        </div>
                    </div>

                    <div className="oco-badges">
                        <span className="oco-badge oco-verde">Rotina</span>
                        {status}
                    </div>
                </div>

                {/* **************** ESTUDANTE E TURMA **************** */}

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

                {/* **************** DESCRIÇÃO **************** */}

                <div className="oco-card">
                    <div className="oco-card-head">
                        <span className="oco-icone">
                            <img
                                src="../../../../public/descriçao.svg"
                                alt=""
                                className="oco-icone-img"
                            />
                            Descrição da Ocorrência
                        </span>
                    </div>

                    <div className="oco-descricao">
                        {dadosOcorrencia?.descricao}
                    </div>
                </div>

                {/* **************** ENCAMINHAR **************** */}

                <div className="oco-card">
                    <div className="oco-card-head">
                        <span className="oco-icone">
                            <img
                                src="../../../../public/Seta.png"
                                alt=""
                                className="oco-icone-img"
                            />
                            Encaminhar Ocorrência
                        </span>
                    </div>

                    <input
                        type="text"
                        placeholder="Descreva aqui o necessário p/ fazer"
                        className="oco-input"
                        value={observacao}
                        onChange={(e) => setObservacao(e.target.value)}
                    />

                    <label className="oco-label">
                        Selecione o destino da ocorrência *
                    </label>

                    <div className="oco-encaminhar-row">

                        {/* TIPO DE DESTINO - MENU PERSONALIZADO */}

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
                                    {tipoDestino === "responsavel"
                                        ? "Responsável do aluno"
                                        : tipoDestino === "coordenador"
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
                                            tipoDestino === "responsavel"
                                                ? "selecionada"
                                                : ""
                                        }`}
                                        onClick={() => {
                                            setTipoDestino("Responsavel");
                                            setIdDestino("");
                                            setBuscaDestino("");
                                            setMostrarOpcoesDestino(false);
                                        }}
                                    >
                                        Responsável do aluno
                                    </button>

                                    <button
                                        type="button"
                                        className={`oco-opcao-destino ${
                                            tipoDestino === "coordenador"
                                                ? "selecionada"
                                                : ""
                                        }`}
                                        onClick={() => {
                                            setTipoDestino("Coordenador");
                                            setIdDestino("");
                                            setBuscaDestino("");
                                            setMostrarOpcoesDestino(false);
                                        }}
                                    >
                                        Coordenador
                                    </button>

                                </div>
                            )}
                        </div>

                        {/* BUSCAR DESTINATÁRIO */}

                        <input
                            type="text"
                            placeholder={
                                tipoDestino === "coordenador"
                                    ? "Buscar coordenador por nome"
                                    : tipoDestino === "responsavel"
                                        ? "Buscar responsável por nome"
                                        : "Selecione o tipo primeiro"
                            }
                            className="oco-input"
                            value={buscaDestino}
                            onChange={(e) => setBuscaDestino(e.target.value)}
                            disabled={!tipoDestino}
                        />

                        {/* BOTÃO ENCAMINHAR */}

                        <button
                            type="button"
                            className="oco-btn-enviar"
                            disabled={!tipoDestino || !buscaDestino.trim()}
                        >
                            <p className="txt-btn-enviar">
                                Encaminhar
                            </p>
                        </button>

                    </div>
                </div>
            </div>
        </div>
    );
};

export default Ocorrencias;
