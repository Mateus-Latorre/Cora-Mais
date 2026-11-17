import { useParams } from "react-router-dom";
import Menu from "../../components/menu/menu";
import "./Ocorrencias.css";
import { useContext, useEffect, useState } from "react";
import { UsuarioContext } from "../../context/UsuarioContext/UsuarioContext";
import api from "../../services/Services";

const Ocorrencias = () => {
    const {idOcorrencia} = useParams();
    const { ususario } = useContext(UsuarioContext)
    const [dadosOcorrencia, setDadosOcorrencia] = useState(null);
    const getOcorrenciaId = async (idOcorrencia) => {
        try {
            const response = await api.get(`/Ocorrencia/${idOcorrencia}`);
            setDadosOcorrencia(response.data);
        }
        catch (error) {
            console.error("Erro ao buscar ocorrência:", error.response?.data || error.message);
        }
    };
    let status = null;
    if (dadosOcorrencia !== null) {
        if (dadosOcorrencia.idOstatus == "ABERTA") {
            status = <span className="oco-badge oco-azul">Aberta</span>;
        }
        else if (dadosOcorrencia.idOstatus == "ANDAMENTO") {
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
                        <span className="oco-icone"><img src="../../../../public/identificacao.svg" alt="" className="oco-icone-img" /> Título da ocorrência</span>
                    <div className="oco-titulo-valor">{dadosOcorrencia?.titulo}</div>
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
                        <div className="oco-valor">Nome do Estudante</div>
                    </div>
                    <div className="oco-box-metade">
                        <label>Turma</label>
                        <div className="oco-valor">Turma</div>
                    </div>
                </div>

                {/* **************** DESCRIÇÃO **************** */}
                <div className="oco-card">
                    <div className="oco-card-head">
                        <span className="oco-icone"><img src="../../../../public/descriçao.svg" alt="" className="oco-icone-img" /> Descrição da Ocorrência</span>
                    </div>
                    <div className="oco-descricao">
                        Descricao da ocorrência
                    </div>
                </div>

                {/* **************** ENCAMINHAR **************** */}
                <div className="oco-card">
                    <div className="oco-card-head">
                        <span className="oco-icone"><img src="../../../../public/Seta.png" alt="" className="oco-icone-img" /> Encaminhar Ocorrência</span>
                    </div>
                    <input
                        type="text"
                        placeholder="Descreva aqui o necessário p/ fazer"

                        className="oco-input"
                    />
                     <label className="oco-label">Selecione o responsável pela ocorrência *</label>
                    <div className="oco-encaminhar-row">
                        <input
                            type="text"
                            placeholder="Buscar por nome"
                            className="oco-input"
                        />
                        <button type="button" className="oco-btn-enviar">
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
