import { Link, Links } from "react-router-dom";
import "./menu.css"

const Menu = () => {
    return (
        <header className="menu">
            <div className="logo">
                <img src="/Logo-Img.svg" alt="a imagem da logo" className="icon-logoI" />
                <img src="/Logo-Texto.svg" alt="a imagem da logo" className="icon-logoT" />
            </div>

            <div className="LinhaLaranja" />

            <div className="rotas">
                <Link className="rota" to="/Feed">
                    <img src="/Img-Casa.svg" alt="imagem pra tela de login" className="icon-menu" />
                    <h2 className="Texto-menu">Inicio</h2>
                </Link>
                <Link className="rota" to="/Notificacoes">
                    <img src="/Img-Notificacao.svg" alt="imagem pra tela de Notificações" className="icon-menu" />
                    <h2 className="Texto-menu">Notificações</h2>
                </Link>

                <Link className="rota" to="/Perfil">
                    <img src="/Img-Perfil-Laran.svg" alt="imagem pra tela de Perfil" className="icon-menu" />
                    <h2 className="Texto-menu">Perfil</h2>
                </Link>

            </div>
            <Link className="rota" to="/">
                <img src="/Img-Sair.svg" alt="imagem pra tela de login" className="icon-menu" />
                <h2 className="Texto-sair">Sair</h2>

            </Link>
        </header>
    )
}

export default Menu;