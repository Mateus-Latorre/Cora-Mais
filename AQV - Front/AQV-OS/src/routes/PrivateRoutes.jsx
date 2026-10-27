import { useContext } from "react";
import { Navigate } from "react-router-dom";
// import { AQVContext } from "../context/AQVContext";

const PrivateRoutes = ({ children }) => {
    const { usuario } = useContext(AQVContext);
    return usuario ? children : <Navigate to="/" />
}
export default PrivateRoutes;