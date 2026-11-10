import "./Perfil.css";
import Menu from "../../components/menu/menu";
import { useContext } from "react";
import { UsuarioContext } from "../../context/UsuarioContext/UsuarioContext";

const Perfil = () => {
  const {usuario} = useContext(UsuarioContext);
    console.log("Payload do Usuário:", JSON.stringify(usuario, null, 2));
  let img = null;
  if (usuario.role === "Professor") {
    img = "../../public/professor.png";
  }else if (usuario.role === "AQV") {
    img = "../../public/aqv.png";
  }else if (usuario.role === "Coordenador") {
    img = "../../public/coordenador.png";
  }
  let imgStyle = null;
  if (usuario.role === "Professor") {
    imgStyle = "imgProf";
  }else if (usuario.role === "AQV") {
    imgStyle = "imgAQV";
  }else if (usuario.role === "Coordenador") {
    imgStyle = "imgCoordenador";
  }
  const [senhaAtual, setSenhaAtual] = useState("");
  const [novaSenha, setNovaSenha] = useState("");
  const [confirmarSenha, setConfirmarSenha] = useState("");
  const [salvando, setSalvando] = useState(false);
  const trocarSenha = async () => {
  if (!senhaAtual || !novaSenha || !confirmarSenha) {
    Alerta({
      title: "Atenção",
      text: "Preencha todos os campos.",
      icon: "warning",
      confirmButtonText: "Ok",
    });
    return;
  }

  if (novaSenha.length < 6) {
    Alerta({
      title: "Atenção",
      text: "A nova senha precisa ter pelo menos 6 caracteres.",
      icon: "warning",
      confirmButtonText: "Ok",
    });
    return;
  }

  if (novaSenha !== confirmarSenha) {
    Alerta({
      title: "Atenção",
      text: "As senhas novas não coincidem.",
      icon: "warning",
      confirmButtonText: "Ok",
    });
    return;
  }

  setSalvando(true);
  try {
    await api.patch(`/Usuario/${usuario.id}/senha`, {
      senhaAtual,
      novaSenha,
    });

    Alerta({
      title: "Sucesso",
      text: "Senha alterada com sucesso!",
      icon: "success",
      confirmButtonText: "Ok",
    });

    setSenhaAtual("");
    setNovaSenha("");
    setConfirmarSenha("");
  } catch (error) {
    Alerta({
      title: "Erro",
      text: error.response?.data || "Não foi possível alterar a senha.",
      icon: "error",
      confirmButtonText: "Ok",
    });
  } finally {
    setSalvando(false);
  }
};
  return (
    <div className="body">
      {/* *******           Menu           ********** */}
      <Menu />
      {/* **************        Tela          ***************** */}
      <div className="container">
        <div className="titulo">
          <h1 className="titulo_l">Perfil</h1>
        </div>

        <div className="caixa">
          <img src={usuario ? img : <p>carregando</p>} alt="" className={imgStyle} />
          <div className="Desc">
            <img src="../../public/editar.png" alt="" className="imgEdit" />
            <div className="Info">
              <img src="../../public/pessoa.svg" alt="" className="imgDesc" />
              <div className="Info_txt">
                <p className="T_desc">Nome</p>
                <p className="txt_desc_nome">{usuario.nome}</p>
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
                <p className="txt_desc">{usuario.email}</p>
              </div>
            </div>
            <div className="Info_tipo">
              <img src="../../public/pessoas.png" alt="" className="imgDesc" />
              <div className="Info_txt">
                <p className="T_desc">Tipo de usuário</p>
                <p className="txt_desc">{usuario.role}</p>
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
              <img
                src="../../public/salvar.png"
                alt=""
                className="salvar_img"
              />
              <p className="salvar_txt">Salvar</p>
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Perfil;
