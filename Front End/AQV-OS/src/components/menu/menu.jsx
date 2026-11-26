
import { Link, useLocation } from "react-router-dom";
import { useState, useEffect } from "react";
import "./menu.css";

const Menu = () => {
<<<<<<< HEAD
    const [menuAberto, setMenuAberto] = useState(false);
    const location = useLocation();

    useEffect(() => {
        setMenuAberto(false);
    }, [location.pathname]);

    useEffect(() => {
        const verificarTela = () => {
            if (window.innerWidth > 768) {
                setMenuAberto(false);
            }
        };

        window.addEventListener("resize", verificarTela);

        return () => {
            window.removeEventListener("resize", verificarTela);
        };
    }, []);
=======
    const sair = () => {
        localStorage.clear();
    };
>>>>>>> 8ae198dfda9a96a73eb2d62638bb8719b30cc19c

    return (
        <>
            {/* BOTÃO MENU RESPONSIVO */}
            <button
                className={`botao-menu-mobile ${menuAberto ? "menu-ativo" : ""}`}
                onClick={() => setMenuAberto(!menuAberto)}
                aria-label={menuAberto ? "Fechar menu" : "Abrir menu"}
                aria-expanded={menuAberto}
            >
                <span></span>
                <span></span>
                <span></span>
            </button>

            {/* FUNDO ESCURO */}
            {menuAberto && (
                <div
                    className="fundo-menu-mobile"
                    onClick={() => setMenuAberto(false)}
                />
            )}

            {/* MENU LATERAL */}
            <header className={`menu ${menuAberto ? "menu-aberto" : ""}`}>

                {/* LOGO */}
                <div className="logo">
                    <img
                        src="/Logo-Img.svg"
                        alt="Logo AQV"
                        className="icon-logoI"
                    />

                    <img
                        src="/Logo-Texto.svg"
                        alt="AQV-OS"
                        className="icon-logoT"
                    />
                </div>

                {/* LINHA LARANJA */}
                <div className="LinhaLaranja" />

                {/* NAVEGAÇÃO */}
                <div className="rotas">

                    <Link className="rota" to="/DashBoard">
                        <img
                            src="/Img-Casa.svg"
                            alt=""
                            className="icon-menu"
                        />

                        <h2 className="Texto-menu">Inicio</h2>
                    </Link>

                    <Link className="rota" to="/Notificacoes">
                        <img
                            src="/Img-Notificacao.svg"
                            alt=""
                            className="icon-menu"
                        />

                        <h2 className="Texto-menu">Notificações</h2>
                    </Link>

                    <Link className="rota" to="/Perfil">
                        <img
                            src="/Img-Perfil-Laran.svg"
                            alt=""
                            className="icon-menu"
                        />

                        <h2 className="Texto-menu">Perfil</h2>
                    </Link>

                </div>

                {/* SAIR */}
                <Link className="rota rota-sair" to="/">
                    <img
                        src="/Img-Sair.svg"
                        alt=""
                        className="icon-menu"
                    />

                    <h2 className="Texto-sair">Sair</h2>
                </Link>

            </header>
        </>
    );
};

<<<<<<< HEAD
export default Menu;
=======
            </div>
            <Link className="rota" to="/" on onClick={sair}>
                <img src="/Img-Sair.svg" alt="imagem pra tela de login" className="icon-menu" />
                <h2 className="Texto-sair">Sair</h2>

            </Link>
        </header>
    )
}

export default Menu;
>>>>>>> 8ae198dfda9a96a73eb2d62638bb8719b30cc19c
