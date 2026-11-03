
import "./Login.css";

function Login() {
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

                <h2 className="form-title">
                    Login
                </h2>
                <p>Bem-vindo de volta, entre na sua conta</p>
                <form className="login-form">

                    <div className="input-wrapper">


                        <input
                            id="email"
                            type="email"
                            placeholder="E-mail"
                            required
                        />
                    </div>

                    <div className="input-wrapper">

                        <input
                            id="password"
                            type="password"
                            placeholder="Senha"
                            required
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