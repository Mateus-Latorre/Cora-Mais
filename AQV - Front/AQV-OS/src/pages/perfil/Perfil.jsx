import "./Perfil.css";
import Menu from "../../components/menu/menu";

const Perfil = () => {
  return (
    <div className="body">
      {/* *******           Menu           ********** */}
      <Menu/>
      {/* **************        Tela          ***************** */}
      <div className="container">
        <div className="titulo">
          <h1 className="titulo_l">Perfil</h1>
        </div>

        <div className="caixa">
          <img src="../../public/professor.png" alt="" className="imgProf" />
          <div className="Desc">
            <div className="Info">
              <img src="../../public/pessoa.svg" alt="" className="imgDesc" />
              <div className="Info_txt">
                <p className="T_desc">Nome</p>
                <p className="txt_desc_nome">Lady Gaga</p>
              </div>
            </div>
            <div className="Info">
              <img
                src="../../public/Email.png"
                alt=""
                className="imgDescEmail"
              />
              <div className="Info_txt">
                <p className="T_desc">E-mail</p>
                <p className="txt_desc">lady@gmail.com</p>
              </div>
            </div>
            <div className="Info">
              <img
                src="../../public/calendario.png"
                alt=""
                className="imgDesc"
              />
              <div className="Info_txt">
                <p className="T_desc">Usuário desde</p>
                <p className="txt_desc">15/10/2025</p>
              </div>
            </div>
            <div className="Info_tipo">
              <img src="../../public/pessoas.png" alt="" className="imgDesc" />
              <div className="Info_txt">
                <p className="T_desc">Tipo de usuário</p>
                <p className="txt_desc">Professora</p>
              </div>
            </div>
          </div>
        </div>
        <div className="titulo2">
          <h1 className="titulo_2_">Trocar de Senha</h1>
        </div>
        <div className="caixa2">
          <div className="campo">
            <div className="campo_senhas_atual">
              <h4 className="campo_txt">Digite sua senha atual</h4>
              <input className="campo_input" type="password" />
            </div>
            <div className="campo_senhas">
              <h4 className="campo_txt">Digite sua nova senha</h4>
              <input className="campo_input" type="password" />
            </div>
            <div className="campo_senhas">
              <h4 className="campo_txt">Confirme sua nova senha</h4>
              <input className="campo_input" type="password" />
            </div>
          </div>
            <div className="btn">
            <button className="btn_salvar">
              <img src="../../public/salvar.png" alt="" className="salvar_img"/>
                <p className="salvar_txt">Salvar</p>
            </button>
            </div>
        </div>
      </div>
    </div>
  );
};

export default Perfil;
