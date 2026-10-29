import { useEffect, useState } from "react";
import "./CriarOcorrencias.css"
import Menu from "../../components/menu/menu";


const NovaOcorrencia = () => {
    return (
        <div className="body">

            { /* *******           Menu           ********** */}

            <Menu></Menu>

            {/* **************        Tela          ***************** */}
            <div className="div">
                <div className="titulo">
                    <p>Nova Ocorrência</p>
                    <h1>Registrar uma nova Ocorrência</h1>
                </div>
                <form action="" >
                    <div className="box">
                        <div className="caixa">
                            <img src="identificacao.svg" alt="" />

                            <h3>Identificação da Ocorrência</h3>
                        </div>
                        <div className="inputs">
                            <div>
                                <h5>Titulo da Ocorrência</h5>

                                <input type="text" placeholder="Ex: Atraso freuqentenas entregas das atividades" />
                            </div>

                            <div>
                                <h5>Status da Ocorrência</h5>
                                <input type="text" placeholder="Aberta" />
                            </div>
                        </div>
                    </div>


                    <div className="box">
                        <div className="caixa">
                            <img src="estudante.svg" alt="" />

                            <h3>Estudante</h3>
                        </div>
                        <div className="inputs">
                            <div>
                                <h5>Selecione o estudante *</h5>
                                <div class="input-container">
                                    <i class="fa-solid fa-user icon"><img src="lupa.svg" alt="" /></i>
                                    <input type="text" placeholder="Buscar por CPF ou nome" />
                                </div>

                            </div>

                            <div>
                                <h5>Turma</h5>

                                <input type="" placeholder="Selecione a Turma" />
                            </div>
                        </div>
                    </div>


                    <div className="boxD">
                        <div className="caixa">
                            <img src="descriçao.svg" alt="" />

                            <h3>Descrição da Ocorrência</h3>
                        </div>
                        <textarea type="text" placeholder="Digite sobre o assunto ..." className="areaText" />
                    </div>

                    <div className="boxD">
                        <div className="caixa">
                            <img src="aviao-papel.svg" alt="" />

                            <h3>Ações</h3>
                        </div>

                        <div className="areaAzul">
                            <button className="btnAzul">Salvar e continuar</button>
                            <button className="btnBranco">Cancelar</button>
                        </div>
                    </div>
                </form>
            </div>
        </div>
    );
};

export default NovaOcorrencia;