import { useContext } from "react";
import { Navigate } from "react-router-dom";
import { UsuarioContext } from "../context/UsuarioContext/UsuarioContext";

const PrivateRoutes = ({children}) => {
    const { usuario } = useContext(UsuarioContext);
    return usuario ? children : <Navigate to="/" />
}
export default PrivateRoutes;