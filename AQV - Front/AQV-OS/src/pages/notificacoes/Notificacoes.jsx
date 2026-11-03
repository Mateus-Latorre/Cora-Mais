import { useEffect, useState } from "react";
import Menu from "../../components/menu/menu";
import "./Notificacoes.css";

const Notificacoes = () => {
  const [aba, setAba] = useState("todas");
  return (
    <div className="body">
      {/* *******           Menu           ********** */}
      <Menu />
      {/* **************        Tela          ***************** */}
      <div className="container">
        <div className="cabecalho">
          <div className="titulo">
            <h1 className="titulo_l">Notificações</h1>
            <p className="subtitulo">
              Acompanhe as ocorrências registradas pela equipe.
            </p>
          </div>
          <div className="btn-lidas">
            <button className="noti_lidas">
              <img src="../../public/lidas.png" alt="" className="lidas_img" />
              <p className="noti-txt">Marcar todas como lidas</p>
            </button>
          </div>
        </div>
        <div className="filtro" role="tablist">
          <button
            role="tab"
            aria-selected={aba === "todas"}
            className={aba === "todas" ? "ativo" : ""}
            onClick={() => setAba("todas")}
          >
            Todas <span>7</span>
          </button>

          <button
            role="tab"
            aria-selected={aba === "naoLidas"}
            className={aba === "naoLidas" ? "ativo" : ""}
            onClick={() => setAba("naoLidas")}
          >
            Não lidas <span>3</span>
          </button>
        </div>
        <div className="notis">
          
          <p className="noti-txt">Hoje</p>
          <div className="campo-todas-notis">
            <div className="campo-noti">
              <div className="campo-noti-img">
                <img src="../../public/sino.png" alt="" className="img-noti" />
              </div>
              <div className="campo-noti-txt">
                <p className="txt-noti">
                  <span className="noti-prof">Professor Eduardo</span> registrou
                  uma ocorrência
                </p>
                <p className="hora-txt">Hoje às 15:30</p>
              </div>
            </div>
            <div className="campo-noti">
              <div className="campo-noti-img">
                <img src="../../public/sino.png" alt="" className="img-noti" />
              </div>
              <div className="campo-noti-txt">
                <p className="txt-noti">
                  <span className="noti-prof">Professor Eduardo</span> registrou
                  uma ocorrência
                </p>
                <p className="hora-txt">Hoje às 15:30</p>
              </div>
            </div>
            <div className="campo-noti">
              <div className="campo-noti-img">
                <img src="../../public/sino.png" alt="" className="img-noti" />
              </div>
              <div className="campo-noti-txt">
                <p className="txt-noti">
                  <span className="noti-prof">Professor Eduardo</span> registrou
                  uma ocorrência
                </p>
                <p className="hora-txt">Hoje às 15:30</p>
              </div>
            </div>
          </div>
          <p className="noti-txt">Ontem</p>
          <div className="campo-todas-notis">
            <div className="campo-noti">
              <div className="campo-noti-img">
                <img src="../../public/sino.png" alt="" className="img-noti" />
              </div>
              <div className="campo-noti-txt">
                <p className="txt-noti">
                  <span className="noti-prof">Professor Eduardo</span> registrou
                  uma ocorrência
                </p>
                <p className="hora-txt">Hoje às 15:30</p>
              </div>
            </div>
            <div className="campo-noti">
              <div className="campo-noti-img">
                <img src="../../public/sino.png" alt="" className="img-noti" />
              </div>
              <div className="campo-noti-txt">
                <p className="txt-noti">
                  <span className="noti-prof">Professor Eduardo</span> registrou
                  uma ocorrência
                </p>
                <p className="hora-txt">Hoje às 15:30</p>
              </div>
            </div>
            <div className="campo-noti">
              <div className="campo-noti-img">
                <img src="../../public/sino.png" alt="" className="img-noti" />
              </div>
              <div className="campo-noti-txt">
                <p className="txt-noti">
                  <span className="noti-prof">Professor Eduardo</span> registrou
                  uma ocorrência
                </p>
                <p className="hora-txt">Hoje às 15:30</p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Notificacoes;
