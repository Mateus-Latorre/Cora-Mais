import { useEffect, useState } from "react";
import Menu from "../../components/menu/menu";

const HistOcorrencias = () => {
    return (
        <>
            <Menu></Menu>

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