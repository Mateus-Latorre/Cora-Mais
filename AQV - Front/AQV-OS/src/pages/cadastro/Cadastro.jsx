
import { useState } from "react";
import "./Cadastro.css";
import { Link, useNavigate } from "react-router-dom";
import { Alerta } from "../../components/alerta/alerta";
import api from "../../services/Services";

function Cadastro() {
    const [showPassword, setShowPassword] = useState(false);
    const [showConfirm, setShowConfirm] = useState(false);

    const [nomeCompleto, setNomeCompleto] = useState("");
    const [email, setEmail] = useState("");
    const [senha, setSenha] = useState("");
    const [confirmarSenha, setConfirmarSenha] = useState("");
    const [numero, setNumero] = useState("");
    const [idTipoUsuario, setIdTipoUsuario] = useState("");

    const navigate = useNavigate();

    // FORMATAR TELEFONE AUTOMATICAMENTE
    const formatarTelefone = (valor) => {
        return valor
            .replace(/\D/g, "")
            .slice(0, 11)
            .replace(/^(\d{2})(\d)/, "($1) $2")
            .replace(/(\d{5})(\d)/, "$1-$2");
    };

    const Cadastro = async (e) => {
        e.preventDefault();

        // Verificar campos vazios
        if (
            nomeCompleto.trim() === "" ||
            email.trim() === "" ||
            senha.trim() === "" ||
            confirmarSenha.trim() === "" ||
            numero.trim() === "" ||
            idTipoUsuario === ""
        ) {
            Alerta({
                title: "Atenção",
                text: "Preencha todos os campos.",
                icon: "warning",
                confirmButtonText: "Ok"
            });

            return;
        }

        // Verificar se as senhas são iguais
        if (senha !== confirmarSenha) {
            Alerta({
                title: "Atenção",
                text: "As senhas não são iguais.",
                icon: "warning",
                confirmButtonText: "Ok"
            });

            return;
        }

        // Dados enviados para a API
        const dadosCadastro = {
            Nome: nomeCompleto.trim(),
            Email: email.trim(),
            Senha: senha,
            Numero: numero,
            IdTipoUsuario: idTipoUsuario
        };

        try {
            const retornoAPI = await api.post("/Usuario", dadosCadastro);

            console.log("Cadastro realizado:", retornoAPI.data);

            Alerta({
                title: "Sucesso!",
                text: "Usuário cadastrado com sucesso.",
                icon: "success",
                confirmButtonText: "Ok"
            });

            // Limpar campos
            setNomeCompleto("");
            setEmail("");
            setSenha("");
            setConfirmarSenha("");
            setNumero("");
            setIdTipoUsuario("");

            navigate("/login");

        } catch (error) {
            console.error(
                "Erro ao cadastrar:",
                error.response?.data ?? error.message
            );

            const mensagemErro =
                typeof error.response?.data === "string"
                    ? error.response.data
                    : "Não foi possível realizar o cadastro. Verifique os dados.";

            Alerta({
                title: "Erro no cadastro",
                text: mensagemErro,
                icon: "error",
                confirmButtonText: "Ok"
            });
        }
    };

    return (
        <div className="cadastro-layout">

            <div className="cadastro-presentation">

                <span className="app-tag">Seu app</span>

                <h1>
                    Comece a ter mais <span>praticidade</span>
                    <br />
                    hoje.
                </h1>

                <p>Entre para continuar de onde parou.</p>

            </div>

            <div className="cadastro-container">

                <h2 className="form-title">Cadastre-se</h2>

                <p>Preencha os dados para criar sua conta</p>

                <form className="cadastro-form" onSubmit={Cadastro}>

                    {/* NOME COMPLETO */}
                    <div className="input-container">

                        <input
                            id="nomeCompleto"
                            type="text"
                            placeholder="Nome Completo"
                            value={nomeCompleto}
                            onChange={(e) =>
                                setNomeCompleto(e.target.value)
                            }
                            required
                        />

                        <img
                            src="/pessoa.svg"
                            alt=""
                            className="icon icon-right icon-static"
                        />

                    </div>

                    {/* E-MAIL */}
                    <div className="input-container">

                        <input
                            id="Email"
                            type="email"
                            placeholder="E-mail"
                            value={email}
                            onChange={(e) =>
                                setEmail(e.target.value)
                            }
                            required
                        />

                        <img
                            src="/email.svg"
                            alt=""
                            className="icon icon-right icon-static"
                        />

                    </div>

                    {/* NÚMERO DE TELEFONE */}
                    <div className="input-container">

                        <input
                            id="Numero"
                            type="tel"
                            placeholder="Número de Telefone"
                            value={numero}
                            onChange={(e) =>
                                setNumero(formatarTelefone(e.target.value))
                            }
                            required
                        />

                        <img
                            src="/bi_telephone.svg"
                            alt=""
                            className="icon icon-right icon-static"
                        />

                    </div>

                    {/* SENHA */}
                    <div className="input-container">

                        <input
                            id="password"
                            type={showPassword ? "text" : "password"}
                            placeholder="Senha"
                            value={senha}
                            onChange={(e) =>
                                setSenha(e.target.value)
                            }
                            required
                        />

                        <img
                            src={
                                showPassword
                                    ? "/olhofechado.svg"
                                    : "/olho.svg"
                            }
                            alt={
                                showPassword
                                    ? "Ocultar senha"
                                    : "Mostrar senha"
                            }
                            className="icon icon-right"
                            onClick={() =>
                                setShowPassword(!showPassword)
                            }
                        />

                    </div>

                    {/* CONFIRMAR SENHA */}
                    <div className="input-container">

                        <input
                            id="confirmarSenha"
                            type={showConfirm ? "text" : "password"}
                            placeholder="Confirme sua senha"
                            value={confirmarSenha}
                            onChange={(e) =>
                                setConfirmarSenha(e.target.value)
                            }
                            required
                        />

                        <img
                            src={
                                showConfirm
                                    ? "/olhofechado.svg"
                                    : "/olho.svg"
                            }
                            alt={
                                showConfirm
                                    ? "Ocultar senha"
                                    : "Mostrar senha"
                            }
                            className="icon icon-right"
                            onClick={() =>
                                setShowConfirm(!showConfirm)
                            }
                        />

                    </div>

                    {/* TIPO DA CONTA */}
                    <div className="input-container">

                        <select
                            id="tipoConta"
                            value={idTipoUsuario}
                            onChange={(e) =>
                                setIdTipoUsuario(e.target.value)
                            }
                            required
                        >

                            <option value="" disabled>
                                Tipo da Conta
                            </option>

                            {/* Substitua pelos IDs reais do banco */}
                            <option value="06f5b34b-2e6e-4049-92fa-1ca3429e414a">
                                AQV
                            </option>

                            <option value="1a45dd44-ef38-4952-8e89-3b0eb4aea0c6">
                                Coordenador
                            </option>

                            <option value="57241a37-6424-4b4b-b235-408940371bfc">
                                Professor
                            </option>

                        </select>

                    </div>

                    {/* BOTÃO CADASTRAR */}
                    <button
                        type="submit"
                        className="cadastro-button"
                    >
                        Cadastre-se
                    </button>

                </form>

                <p className="signup-text">
                    Já tem conta? <Link to="/login">Entrar</Link>
                </p>

            </div>

        </div>
    );
}

export default Cadastro;
