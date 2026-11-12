import "./Alerta.css";
import Swal from 'sweetalert2'
	export const Alerta = ({
  title,
  text,
  icon,
  showCancelButton = false,
  confirmButtonText = "Ok",
  cancelButtonText = "Cancelar",
  confirmButtonColor = "#3085d6",
  cancelButtonColor = "#d33",
}) => {
  return Swal.fire({
    title,
    text,
    icon,
    showCancelButton,
    confirmButtonText,
    cancelButtonText,
    confirmButtonColor,
    cancelButtonColor,
    customClass: {
      popup: "alerta-popup",
      title: "alerta-title",
      htmlContainer: "alerta-text",
      confirmButton: "alerta-confirm-btn",
      cancelButton: "alerta-cancel-btn",
      icon: "alerta-icon",
    },
    buttonsStyling: false, // desliga o estilo padrão do Swal pros botões, pra sua classe CSS mandar 100%
  });
};
