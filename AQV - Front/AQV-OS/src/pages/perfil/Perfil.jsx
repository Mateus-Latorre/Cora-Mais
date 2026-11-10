import "./Perfil.css";
import Menu from "../../components/menu/menu";
import { useContext, useState } from "react";
import { UsuarioContext } from "../../context/UsuarioContext/UsuarioContext";
import { Alerta } from "../../components/alerta/alerta";
import api from "../../services/Services";

const Perfil = () => {
  const { usuario } = useContext(UsuarioContext);
  let img = null;
  if (usuario.role === "Professor") {
    img = "../../public/professor.png";
  } else if (usuario.role === "AQV") {
    img = "../../public/aqv.png";
  } else if (usuario.role === "Coordenador") {
    img = "../../public/coordenador.png";
  }
  let imgStyle = null;
  if (usuario.role === "Professor") {
    imgStyle = "imgProf";
  } else if (usuario.role === "AQV") {
    imgStyle = "imgAQV";
  } else if (usuario.role === "Coordenador") {
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
    console.log("novaSenha:", JSON.stringify(novaSenha));
    console.log("confirmarSenha:", JSON.stringify(confirmarSenha));
    console.log("são iguais?", novaSenha === confirmarSenha);
    const dadosSenha = {
      SenhaAtual: senhaAtual,
      NovaSenha: novaSenha,
      ConfirmarNovaSenha: confirmarSenha,
    };
    try {
      await api.patch(`/Usuario/${usuario.id}/senha`, dadosSenha);

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
      const mensagemErro = error.response?.data?.errors
        ? Object.values(error.response.data.errors).flat().join(" ")
        : (error.response?.data?.title ??
          error.response?.data?.message ??
          (typeof error.response?.data === "string"
            ? error.response.data
            : null) ??
          "Não foi possível alterar a senha.");

      Alerta({
        title: "Erro",
        text: mensagemErro,
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
          <img
            src={usuario ? img : <p>carregando</p>}
            alt=""
            className={imgStyle}
          />
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
              <input
                className="campo_input"
                type="password"
                value={senhaAtual}
                onChange={(e) => setSenhaAtual(e.target.value)}
              />
            </div>
            <div className="campo_senhas">
              <h4 className="campo_txt">Digite sua nova senha</h4>
              <input
                className="campo_input"
                type="password"
                value={novaSenha}
                onChange={(e) => setNovaSenha(e.target.value)}
              />
            </div>
            <div className="campo_senhas">
              <h4 className="campo_txt">Confirme sua nova senha</h4>
              <input
                className="campo_input"
                type="password"
                value={confirmarSenha}
                onChange={(e) => setConfirmarSenha(e.target.value)}
              />
            </div>
          </div>
          <div className="btn">
            <button
              className="btn_salvar"
              onClick={trocarSenha}
              disabled={salvando}
              type="button"
            >
              <img src="/salvar.png" alt="" className="salvar_img" />
              <p className="salvar_txt">
                {salvando ? "Salvando..." : "Salvar"}
              </p>
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Perfil;
