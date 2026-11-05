import { BrowserRouter, Route, Routes } from "react-router-dom"
import Login from "../pages/login/Login.jsx"
import CadartroUsuario from "../pages/cadastro/Cadastro.jsx"
import CadastroOcorrencia from "../pages/criarOcorrencias/CriarOcorrencias.jsx"
import Notificacoes from "../pages/notificacoes/Notificacoes.jsx"
import Perfil from "../pages/perfil/Perfil.jsx"
import Ocorrencias from "../pages/ocorrencias/Ocorrencias.jsx"
import HistOcorrencias from "../pages/histOcorrencias/HistOcorrencias.jsx"
// import PrivateRoutes from "../routes/PrivateRoutes"

const Rotas = () => {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<Login />} />

                <Route path="/Cadastro" element={<CadartroUsuario />} />

                <Route path="/CadastroOcorrencia" element={<CadastroOcorrencia />} />

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