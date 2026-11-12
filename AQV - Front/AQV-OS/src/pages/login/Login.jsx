import { useContext, useState } from "react";
import "./Login.css";
import { Link, useNavigate } from "react-router-dom";
import { UsuarioContext } from "../../context/UsuarioContext/UsuarioContext";
import { Alerta } from "../../components/alerta/alerta";
import api from "../../services/Services";
import { jwtDecode } from "jwt-decode";

function Login() {
  const [showPassword, setShowPassword] = useState(false);
  const [email, setEmail] = useState("");
  const [senha, setSenha] = useState("");
  const { setUsuario } = useContext(UsuarioContext);
  const navigate = useNavigate();
  const login = async () => {
    if (email.trim().length === 0 || senha.trim().length === 0) {
      Alerta({
        title: "Atenção",
        text: "O login não pode estar em branco.",
        icon: "warning",
        confirmButtonText: "Ok",
      });
      return;
    }
    const dadosLogin = {
      email: email,
      senha: senha,
    };
    try {
      const retornoAPI = await api.post("/Login", dadosLogin);
      const token = retornoAPI.data.token;
      const usuarioDecoded = jwtDecode(token);
      setUsuario(usuarioDecoded);
      localStorage.setItem("usuario", JSON.stringify(usuarioDecoded));
      localStorage.setItem("token", token);
      navigate("/DashBoard");
      setEmail("");
      setSenha("");
    } catch (error) {
      Alerta({
        title: "Login",
        text: "Usuário não encontrado",
        icon: "error",
        confirmButtonText: "Ok",
      });
      console.error("Erro ao fazer login:", error);
    }
  };
  return (
    <div className="login-layout">
      <div className="login-presentation">
        <span className="app-tag">Seu app</span>

        <h1>
          Mais <span>praticidade</span>
          <br />
          no dia a dia
        </h1>

        <p>Entre para continuar de onde parou</p>
      </div>

      <div className="login-container">
        <h2 className="form-title">Login</h2>
        <p>Bem-vindo de volta, entre na sua conta</p>

        <form className="login-form">
          <div className="input-wrapper">
            <input
              id="email"
              type="email"
              placeholder="E-mail"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
            />
            <img src="/email.svg" alt="" className="icon" />
          </div>

          <div className="input-wrapper">
            <input
              id="password"
              type={showPassword ? "text" : "password"}
              placeholder="Senha"
              value={senha}
              onChange={(e) => setSenha(e.target.value)}
              required
            />
            <img
              src={showPassword ? "/olhofechado.svg" : "/olho.svg"}
              alt={showPassword ? "Ocultar senha" : "Mostrar senha"}
              className="icon clickable"
              onClick={() => setShowPassword(!showPassword)}
            />
          </div>

          <button type="button" className="login-button" onClick={login}>
            Entrar
          </button>
        </form>
      </div>
    </div>
  );
}

export default Login;
