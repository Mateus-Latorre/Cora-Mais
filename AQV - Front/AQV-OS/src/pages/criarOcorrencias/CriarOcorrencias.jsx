import { useEffect, useState } from "react";
import "./CriarOcorrencias.css"


const NovaOcorrencia = () => {
    return (
        <div className="body">
            <header className="menu">
                <div className="logo">
                    <img src="/Logo-Img.svg" alt="a imagem da logo" />
                    <img src="/Logo-Texto.svg" alt="a imagem da logo" />
                </div>

                <div className="LinhaLaranja" />

                <div className="rotas">
                    <div>
                        <img src="/Img-Casa.svg" alt="imagem pra tela de login" className="icon-menu" />
                        <h2 className="Texto-menu">Inicio</h2>
                    </div>
                    <div>
                        <img src="/Img-Notificacao.svg" alt="imagem pra tela de Notificações" className="icon-menu" />
                        <h2 className="Texto-menu">Notificações</h2>
                    </div>

                    <div>
                        <img src="/Img-Perfil-Laran.svg" alt="imagem pra tela de Perfil" className="icon-menu" />
                        <h2 className="Texto-menu">Perfil</h2>
                    </div>

                </div>
                <div>
                    <img src="/Img-Sair.svg" alt="imagem pra tela de login" className="icon-menu" />
                    <h2 className="Texto-menu">Perfil</h2>

                </div>
            </header>
            <div >
                <h3>Nova Ocorrência</h3>
                <h1>Registrar uma nova Ocorrência</h1>

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

                                <input type="text" placeholder="Buscar por CPF ou nome" />
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