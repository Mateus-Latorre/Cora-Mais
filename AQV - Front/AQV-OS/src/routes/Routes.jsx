import { BrowserRouter, Route, Routes } from "react-router-dom"
import Login from "../pages/login/Login"
import CadartroUsuario from "../pages/cadastro/Cadastro"
import CadartroOcorrencia from "../pages/criarOcorrencias/CriarOcorrencias"
import Notificacoes from "../pages/notificacoes/Notificacoes"
import Perfil from "../pages/perfil/Perfil"
import Ocorrencias from "../pages/ocorrencias/Ocorrencias"
import HistOcorrencias from "../pages/histOcorrencias/HistOcorrencias"
// import PrivateRoutes from "../routes/PrivateRoutes"

const Rotas = () => {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<Login />} />

                <Route path="/Cadastro" element={<CadartroUsuario />} />

                <Route path="/CadartroOcorrencia" element={<CadartroOcorrencia />} />

                <Route path="/Notificacoes" element={<Notificacoes />} />

                <Route path="/Perfil" element={<Perfil />} />

                <Route path="/Ocorrencias" element={<Ocorrencias />} />

                <Route path="/Feed" element={<HistOcorrencias />} />

                {/* <Route path="/livro" element={
                    <PrivateRoutes>
                        <CadastroLivro />
                    </PrivateRoutes>
                } /> */}

            </Routes>
        </BrowserRouter>
    )
}
export default Rotas