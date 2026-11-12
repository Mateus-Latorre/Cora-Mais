import { useEffect, useState } from "react";
import Menu from "../../components/menu/menu";
import './HistOcorrencias.css';
import { Link } from "react-router-dom";

const HistOcorrencias = () => {

        return (
        <div className="body">
            <Menu />

            <main className="container-ocorrencias">


                {/* BOTÃO */}
                <div className="div-Botao">
                    <h1>Ocorrências</h1>

                    <Link to="/CadastroOcorrencia" className="button-criar">+ Nova ocorrência</Link>
                </div>

                {/* CARDS */}
                <div className="etapas-ocorrencia">

                    <article className="card">
                        <div className="caxa">
                            <div className="indicador total"></div>
                            <h4>Total</h4>
                        </div>
                        <p>8</p>
                    </article>

                    <article className="card">
                        <div className="caxa">
                            <div className="indicador abertas"></div>
                            <h4>Abertas</h4>
                        </div>
                        <p>3</p>
                    </article>

                    <article className="card">
                        <div className="caxa">
                            <div className="indicador EmAndamento"></div>
                            <h4>Em andamento</h4>
                        </div>
                        <p>2</p>
                    </article>

                    <article className="card">
                        <div className="caxa">
                            <div className="indicador finalizadas"></div>
                            <h4>Finalizadas</h4>
                        </div>
                        <p>3</p>
                    </article>

                </div>

                {/* PESQUISA E FILTROS */}
                <div className="filtros-ocorrencias">

                    <div className="input-container">
                        <i className="fa-solid fa-user icon"><img src="lupa.svg" alt="" /></i>
                        <input type="text" placeholder="Buscar por CPF ou nome" />
                    </div>

                    <div className="input-wrapper">
                        <select
                            id="tipoConta"
                            required
                            defaultValue=""
                        >
                            <option value="" disabled>
                                Status
                            </option>
                            <option>Aberta</option>
                            <option>Em andamento</option>
                            <option>Finalizada</option>

                        </select>
                    </div>

                    <input type="date" />

                </div>

                {/* TABELA */}

                <div className="tabela-cabecalho">
                    <span>OCORRÊNCIAS</span>
                    <span>PROFESSOR</span>
                    <span>DATA</span>
                    <span>STATUS</span>
                </div>

                {/* <div className="tabela-ocorrencias">

                  

                    <div className="tabela-conteudo">
                        {/* Aqui serão adicionadas as ocorrências */}



                <div className="tabela-conteudo">

                    <Link to="/Ocorrencias" className="informacoes-ocorrencia ">
                        <div className="descri">
                            <strong>Desrespeitou o professor</strong>
                            <p>Maria Eduarda · DEV-Torloni</p>
                        </div>
                        <div className="prof">Prof. Marcus Vinícius</div>
                        <div className="data">13/10</div>
                        <div className="status aberta">
                            <span></span>
                            <strong>Abertas</strong>
                        </div>
                    </Link>

                    <Link to="/Ocorrencias" className="informacoes-ocorrencia">
                        <div className="descri">
                            <strong>Uso de boné em sala</strong>
                            <p>Mayara Gussonato · DEV-Torloni</p>
                        </div>
                        <div className="prof">Prof. Eduardo Costa</div>
                        <div className="data">13/10</div>
                        <div className="status andamento">
                            <span></span>
                            <strong>Em andamento</strong>
                        </div>
                    </Link>

                    <Link to="/Ocorrencias" className="informacoes-ocorrencia">
                        <div className="descri">
                            <strong>Chegou ao limite de faltas</strong>
                            <p>Matheus Lima · DEV-Torloni</p>
                        </div>
                        <div className="prof">Prof. Marcus Vinícius</div>
                        <div className="data">13/10</div>
                        <div className="status andamento">
                            <span></span>
                            <strong>Em andamento</strong>
                        </div>
                    </Link>

                    <Link to="/Ocorrencias" className="informacoes-ocorrencia">
                        <div className="descri">
                            <strong>Acessou Roblox em sala</strong>
                            <p >Mateus Latorre · DEV-Torloni</p>
                        </div>
                        <div className="prof">Prof. Eduardo Costa</div>
                        <div className="data">13/10</div>
                        <div className="status andamento">
                            <span></span>
                            <strong>Em andamento</strong>
                        </div>
                    </Link>

                    <Link to="/Ocorrencias" className="informacoes-ocorrencia">
                        <div className="descri">
                            <strong>Acessou WhatsApp em sala</strong>
                            <p>Laura Kauany · DEV-Torloni</p>
                        </div>
                        <div className="prof">Prof. Eduardo Costa</div>
                        <div className="data">13/10</div>
                        <div className="status finalizada">
                            <span></span>
                            <strong>Finalizado</strong>
                        </div>
                    </Link>

                </div>

            </main>


        </div>
    );
};

export default HistOcorrencias;