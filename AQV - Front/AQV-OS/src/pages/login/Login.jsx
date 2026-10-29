
import "./Login.css";

function Login() {
    return (
        <div className="login-layout">

            <div className="login-presentation">
                <span className="app-tag">Seu app</span>

                <h1>
                    Mais <span>praticidade</span>
                    <br />
                    no dia a dia.
                </h1>

                <p>Entre para continuar de onde parou.</p>
            </div>

            <div className="login-container">

                <h2 className="form-title">
                    Login
                </h2>
                <p>Bem-vindo de volta, entre na sua conta</p>
                <form className="login-form">

                    <div className="input-wrapper">
                        <label htmlFor="email">E-mail</label>

                        <input
                            id="email"
                            type="email"
                            placeholder="Digite seu e-mail"
                            required
                        />
                    </div>

                    <div className="input-wrapper">
                        <label htmlFor="password">Senha</label>

                        <input
                            id="password"
                            type="password"
                            placeholder="Digite sua senha"
                            required
                        />
                    </div>

                    <a href="#recuperar" className="forgot-password">
                        Esqueci minha senha
                    </a>

                    <button type="submit" className="login-button">
                        Entrar
                    </button>

                </form>

                <p className="signup-text">
                    Não tem uma conta?{" "}
                    <a href="#cadastro">Cadastre-se</a>
                </p>

            </div>
        </div>
    );
}

export default Login;