import "./Perfil.css"
import Menu from "../../components/menu/menu";

const Perfil = () => {
    return (
       <div className="body">

            { /* *******           Menu           ********** */}

            <Menu></Menu>

            {/* **************        Tela          ***************** */}
            <div className="container">
            <div className="titulo">
                <h1 className="titulo_l">Perfil</h1>
            </div>

            <div className="caixa">
                <img src="../../public/professor.png" alt="" className="imgProf"/>
            </div>
            </div>







            </div>
    );
};

export default Perfil;