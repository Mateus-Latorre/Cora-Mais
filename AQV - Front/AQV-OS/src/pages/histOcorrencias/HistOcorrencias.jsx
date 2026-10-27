import { useEffect, useState } from "react";

const HistOcorrencias = () => {
    return (
        <>
            <header className="menu">
                <img src="/Logo-Img.svg" alt="a imagem da logo" />
                <img src="/Logo-Texto.svg" alt="a imagem da logo" />

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
                    <img src="/Img-Sair.svg" alt="imagem pra tela de Perfil" className="icon-menu" />
                    <h2 className="Texto-menu">Perfil</h2>

                </div>
            </header>

            <div className="button-criar">
                <button> Nova corrência</button>
            </div>

            <div className="etapas-ocorrencia">
                <article>
                    <div></div>
                    <h3>Total</h3>
                </article>

                <article>
                    <div></div>
                    <h3>Abertas</h3>
                </article>

                <article>
                    <div></div>
                    <h3>Em andamento</h3>
                </article>

                <article>
                    <div></div>
                    <h3>FInalizadas</h3>
                </article>
            </div>


            <footer></footer>
        </>
    );
};

export default HistOcorrencias;