import Swal from "sweetalert2";
import "./Alerta.css";

export const Alerta = ({
    title = "Aviso",
    text = "",
    icon = "info",
    showCancelButton = false,
    confirmButtonText = "Entendi",
    cancelButtonText = "Cancelar",
    customClass = {},
    ...options
} = {}) => {

    return Swal.fire({
        ...options,

        title,
        text,
        icon,

        showCancelButton,
        confirmButtonText,
        cancelButtonText,

buttonsStyling: false,
background: "#00043A",
color: "#D8EBF3",

customClass: {
    popup: "alerta-popup",
    title: "alerta-title",
    htmlContainer: "alerta-text",
    actions: "alerta-actions",
    confirmButton: "alerta-confirm-btn",
    cancelButton: "alerta-cancel-btn",
    icon: "alerta-icon"
}
    });
};