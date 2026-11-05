import Menu from "../../components/menu/menu";
import "./Ocorrencias.css";

const Ocorrencias = () => {
    return (
        <div className="oco-body">
            <Menu />

            <div className="oco-div">
                <div className="oco-breadcrumb">Ocorrências</div>
                <h1 className="oco-h1">Detalhes da ocorrência</h1>

                {/* **************** TÍTULO + BADGES **************** */}
                <div className="oco-card">
                    <div className="oco-card-head">
                        <span className="oco-icone"><img src="identificacao.svg" alt="" className="oco-icone-img" /> Título da ocorrência</span>
                        <div className="oco-badges">
                            <span className="oco-badge oco-verde">Rotina</span>
                            <span className="oco-badge oco-azul">Aberta</span>
                        </div>
                    </div>
                    <div className="oco-titulo-valor">Título da ocorrência</div>
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
                        <span className="oco-icone"><img src="descriçao.svg" alt="" className="oco-icone-img" /> Descrição da Ocorrência</span>
                    </div>
                    <div className="oco-descricao">
                        aaaaasdddafffddddsdaadadqdpqdqdwqdwqdwqwdwqdwwqdq
                        wqdwwqdqdwqdwwqdqdwqdwwqdqdwqwddqwdqwddqwddqwddqwd
                        wqdwwqdqdwqdwwqdqdw
                    </div>
                </div>

                {/* **************** ENCAMINHAR **************** */}
                <div className="oco-card">
                    <div className="oco-card-head">
                        <span className="oco-icone"><img src="Seta.png" alt="" className="oco-icone-img" /> Encaminhar Ocorrência</span>
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
                            Encaminhar
                        </button>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default Ocorrencias;
