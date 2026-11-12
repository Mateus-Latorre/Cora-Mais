
import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./index.css";
import App from "./App.jsx";

import { UsuarioProvider } from "./context/UsuarioContext/UsuarioProvider.jsx";
import { AQVProvider } from "./context/AQVContext/AQVProvider.jsx";

createRoot(document.getElementById("root")).render(
  <StrictMode>
    <UsuarioProvider>
      <AQVProvider>
        <App />
      </AQVProvider>
    </UsuarioProvider>
  </StrictMode>
);
