import { useState } from "react";
import "./Login.css";

function Login() {
    const [showPassword, setShowPassword] = useState(false);

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
                            required
                        />
                        <img src="/email.svg" alt="" className="icon" />
                    </div>

                    <div className="input-wrapper">
                        <input
                            id="password"
                            type={showPassword ? "text" : "password"}
                            placeholder="Senha"
                            required
                        />
                        <img
                            src={showPassword ? "/olhofechado.svg" : "/olho.svg"}
                            alt={showPassword ? "Ocultar senha" : "Mostrar senha"}
                            className="icon clickable"
                            onClick={() => setShowPassword(!showPassword)}
                        />
                    </div>

                    <button type="submit" className="login-button">
                        Entrar
                    </button>
                </form>
            </div>
        </div>
    );
}

export default Login;