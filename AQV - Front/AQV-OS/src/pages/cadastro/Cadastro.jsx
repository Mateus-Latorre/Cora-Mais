import { useState } from "react";
import "./Cadastro.css";

function Cadastro() {
    const [showPassword, setShowPassword] = useState(false);
    const [showConfirm, setShowConfirm] = useState(false);
    return (
        <div className="cadastro-layout">
            <header>
                <h1>Cadastrar usuario</h1>
            </header>

            <div className="cadastro-presentation">
                <span className="app-tag">Seu app</span>

                <h1>
                    Comece a ter mais <span>praticidade</span>
                    <br />
                    hoje.
                </h1>

                <p>Entre para continuar de onde parou.</p>
            </div>

            <div className="cadastro-container">
                <h2 className="form-title">Cadastre-se</h2>

                <p>Preencha os dados para criar sua conta</p>

                <form className="cadastro-form">
                    {/* NOME COMPLETO */}
                    <div className="input-container">
                        <input
                            id="nomeCompleto"
                            type="text"
                            placeholder="Nome Completo"
                            required
                        />
                        <img src="/pessoa.svg" alt="" className="icon icon-right icon-static" />
                    </div>

                    {/* E-MAIL */}
                    <div className="input-container">
                        <input
                            id="email"
                            type="email"
                            placeholder="E-mail"
                            required
                        />
                        <img src="/email.svg" alt="" className="icon icon-right icon-static" />
                    </div>

                    {/* SENHA */}
                    <div className="input-container">
                        <input
                            id="password"
                            type={showPassword ? "text" : "password"}
                            placeholder="Senha"
                            required
                        />
                        <img
                            src={showPassword ? "/olhofechado.svg" : "/olho.svg"}
                            alt={showPassword ? "Ocultar senha" : "Mostrar senha"}
                            className="icon icon-right"
                            onClick={() => setShowPassword(!showPassword)}
                        />
                    </div>

                    {/* CONFIRMAR SENHA */}
                    <div className="input-container">
                        <input
                            id="confirmarSenha"
                            type={showConfirm ? "text" : "password"}
                            placeholder="Confirme sua senha"
                            required
                        />
                        <img
                            src={showPassword ? "/olhofechado.svg" : "/olho.svg"}
                            alt={showConfirm ? "Ocultar senha" : "Mostrar senha"}
                            className="icon icon-right"
                            onClick={() => setShowConfirm(!showConfirm)}
                        />
                    </div>

                    {/* TIPO DA CONTA */}
                    <div className="input-container">
                        <select id="tipoConta" required defaultValue="">
                            <option value="" disabled>
                                Tipo da Conta
                            </option>
                            <option value="AQV">AQV</option>
                            <option value="Coordenador">Coordenador</option>
                            <option value="Professor">Professor</option>
                        </select>
                    </div>

                    {/* BOTÃO */}
                    <button type="submit" className="cadastro-button">
                        Cadastre-se
                    </button>
                </form>

                <p className="signup-text">
                    Já tem conta? <a href="/login">Entrar</a>
                </p>
            </div>
        </div>
    );
}

export default Cadastro;