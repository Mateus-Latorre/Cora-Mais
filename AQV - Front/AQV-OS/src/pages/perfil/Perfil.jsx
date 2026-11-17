import "./Perfil.css";
import Menu from "../../components/menu/menu";
import { useContext, useState } from "react";
import { UsuarioContext } from "../../context/UsuarioContext/UsuarioContext";
import { Alerta } from "../../components/alerta/alerta";
import api from "../../services/Services";

const Perfil = () => {
  const { usuario } = useContext(UsuarioContext);

  // Estados da troca de senha
  const [senhaAtual, setSenhaAtual] = useState("");
  const [novaSenha, setNovaSenha] = useState("");
  const [confirmarSenha, setConfirmarSenha] = useState("");
  const [salvando, setSalvando] = useState(false);

  // Verifica se o usuário já foi carregado
  if (!usuario) {
    return (
      <div className="body">
        <Menu />
        <div className="container">
          <p>Carregando dados do perfil...</p>
        </div>
      </div>
    );
  }

  // Recupera o perfil e o ID do usuário
  const perfil = usuario.role ?? usuario.perfil;
  const idUsuario = usuario.idUsuario ?? usuario.id;

  // Imagens de acordo com o perfil
  const imagens = {
    Professor: "/professor.svg.svg",
    AQV: "/aqv.svg.svg",
    Coordenador: "/coordenador.svg.svg",
  };

  const classes = {
    Professor: "imgProf",
    AQV: "imgAQV",
    Coordenador: "imgCoordenador",
  };

  const img = imagens[perfil] ?? "/professor.svg.svg";
  const imgStyle = classes[perfil] ?? "";

  // Função para trocar a senha
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

    const dadosSenha = {
      SenhaAtual: senhaAtual,
      NovaSenha: novaSenha,
      ConfirmarNovaSenha: confirmarSenha,
    };

    try {
      await api.patch(`/Usuario/${idUsuario}/senha`, dadosSenha);

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
      <Menu />

      <div className="container">
        <div className="titulo">
          <h1 className="titulo_l">Perfil</h1>
        </div>

        <div className="caixa">
          <img src={img} alt="Foto do perfil" className={imgStyle} />

          <div className="Desc">
            <img src="/editar.png" alt="" className="imgEdit" />

            <div className="Info">
              <img src="/pessoa.svg" alt="" className="imgDesc" />

              <div className="Info_txt">
                <p className="T_desc">Nome</p>
                <p className="txt_desc_nome">{usuario.nome}</p>
              </div>
            </div>

            <div className="Info">
              <img src="/Email.svg.svg" alt="" className="imgDescEmail" />

              <div className="Info_txt">
                <p className="T_desc">E-mail</p>
                <p className="txt_desc">{usuario.email}</p>
              </div>
            </div>

            <div className="Info">
              <img src="/iphone.svg.svg" alt="" className="imgDesc" />

              <div className="Info_txt">
                <p className="T_desc">Usuário desde</p>
                <p className="txt_desc">15/10/2025</p>
              </div>
            </div>

            <div className="Info_tipo">
              <img src="/pessoas.svg.svg" alt="" className="imgDesc" />

              <div className="Info_txt">
                <p className="T_desc">Tipo de usuário</p>
                <p className="txt_desc">{perfil}</p>
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
              <img src="/salvar.svg.svg" alt="" className="salvar_img" />

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
