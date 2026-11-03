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
                <div className="rota">
                    <img src="/Img-Casa.svg" alt="imagem pra tela de login" className="icon-menu" />
                    <h2 className="Texto-menu">Inicio</h2>
                </div>
                <div className="rota">
                    <img src="/Img-Notificacao.svg" alt="imagem pra tela de Notificações" className="icon-menu" />
                    <h2 className="Texto-menu">Notificações</h2>
                </div>

                <div className="rota">
                    <img src="/Img-Perfil-Laran.svg" alt="imagem pra tela de Perfil" className="icon-menu" />
                    <h2 className="Texto-menu">Perfil</h2>
                </div>

            </div>
            <div className="rota">
                <img src="/Img-Sair.svg" alt="imagem pra tela de login" className="icon-menu" />
                <h2 className="Texto-sair">Sair</h2>

            </div>
        </header>
    )
}

export default Menu;