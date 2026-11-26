import { useContext, useEffect, useState } from "react";
import "./CriarOcorrencias.css"
import Menu from "../../components/menu/menu";
import { useNavigate } from "react-router-dom";
import { UsuarioContext } from "../../context/UsuarioContext/UsuarioContext";
import api from "../../services/Services";
import Swal from "sweetalert2";

const Alerta = Swal.fire;

const NovaOcorrencia = () => {
    const [alunoSelecionado, setAlunoSelecionado] = useState(null);
    const [visibilidade, setVisibilidade] = useState(false);
    const [titulo, setTitulo] = useState("");
    const [listaAluno, setListaAluno] = useState([]);
    const [busca, setBusca] = useState("");
    const [IdAluno, setIdAluno] = useState("");
    const [descricao, setDescricao] = useState("");
    const [prioridade, setPrioridade] = useState("");
    const { usuario } = useContext(UsuarioContext);

    const getAlunos = async () => {
        try {
            // debugger;
            const retornoAPI = await api.get("/Aluno");

            console.log(retornoAPI.data);

            setListaAluno(retornoAPI.data);
        } catch (error) {
            console.log(error);

            Alerta({
                title: "Erro",
                text: "Erro ao buscar alunos!",
                icon: "error",
            });
        }
    };

    const cadastrarOcorrencia = async () => {
        try {
            const token = localStorage.getItem("token");

            const ocorrenciaDados = {
                titulo: titulo,
                descricao: descricao,
                idAluno: IdAluno,
                prioridade: prioridade,
                anexo: null
            };

            const resposta = await api.post("/Ocorrencia", ocorrenciaDados,
                {
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                }
            );

            alert("Ocorrência cadastrada com sucesso!");

            setTitulo("");
            setDescricao("");
            setPrioridade("");

        } catch (erro) {
            console.error(
                "Erro:",
                erro.response?.data || erro.message
            );
            console.log(usuario.perfil)

            alert("Erro ao cadastrar ocorrência!");
        }
    };

    // Filtrar Alunos

    const alunosEncontrados = listaAluno.filter((aluno) => {
        const nome = String(aluno?.nome || "").toLowerCase();
        const cpf = String(aluno?.cpf || "").replace(/\D/g, "");

        const pesquisa = busca.trim().toLowerCase();
        const pesquisaCPF = busca.replace(/\D/g, "");

        return (
            aluno &&
            pesquisa.length > 0 &&
            (
                nome.includes(pesquisa) ||
                (pesquisaCPF.length > 0 && cpf.includes(pesquisaCPF))
            )
        );
    });

    useEffect(() => {
        getAlunos();
    }, []);



    return (
        <div className="fundo-pagina criar-ocorrencia-body">

            { /* *******           Menu           ********** */}

            <Menu></Menu>

            {/* **************        Tela          ***************** */}
            <div className="div">
                <div className="titulo">
                    <p>Nova Ocorrência</p>
                    <h1>Registrar uma nova Ocorrência</h1>
                </div>
                <form onSubmit={(e) => {
                    e.preventDefault();
                    cadastrarOcorrencia();
                }}>
                    <div className="box">
                        <div className="caixa-icons">
                            <img src="identificacao.svg" alt="" />

                            <h3>Identificação da Ocorrência</h3>
                        </div>
                        <div className="inputs">
                            <div className="input">
                                <h5>Titulo da Ocorrência</h5>

                                <input type="text" placeholder="Ex: Atraso freuqentenas entregas das atividades" value={titulo || ""} onChange={(e) => setTitulo(e.target.value)} />
                            </div>

                            <div className="input">
                                <h5>Prioridade</h5>
                                <select
                                    value={prioridade}
                                    onChange={(e) => setPrioridade(e.target.value)}
                                >
                                    <option value="" disabled> Selecione </option>

                                    <option value="Rotina"> Rotina </option>
                                    <option value="Importante"> Importante </option>
                                    <option value="Urgente"> Urgente </option>
                                </select>
                            </div>
                        </div>
                    </div>


                    <div className="box">
                        <div className="caixa-icons">
                            <img src="estudante.svg" alt="" />

                            <h3>Estudante</h3>
                        </div>
                        <div className="inputs">
                            <div className="input">
                                <h5>Selecione o estudante *</h5>
                                <div className="input-container">

                                    <i className="fa-solid fa-user icon">
                                        <img src="lupa.svg" alt="" />
                                    </i>

                                    <input
                                        type="text"
                                        placeholder="Buscar por CPF ou nome"
                                        value={busca}
                                        onChange={(e) => {
                                            setBusca(e.target.value);
                                            setAlunoSelecionado(null);
                                            setIdAluno("");
                                            setVisibilidade(true);
                                        }}
                                        onFocus={() => setVisibilidade(true)}
                                    />

                                    {visibilidade && !alunoSelecionado && busca.trim() !== "" && (
                                        <section className="list-alu">

                                            {alunosEncontrados.length > 0 ? (

                                                alunosEncontrados.map((alu) => (
                                                    <button
                                                        type="button"
                                                        className="alu"
                                                        key={alu.idAluno}
                                                        onClick={() => {
                                                            setAlunoSelecionado(alu);
                                                            setIdAluno(alu.idAluno);
                                                            setBusca(alu.nome);
                                                            setVisibilidade(false);
                                                        }}
                                                    >
                                                        <strong>{alu.nome}</strong>
                                                        <span>CPF: {alu.cpf || "Não informado"}</span>
                                                    </button>
                                                ))

                                            ) : (

                                                <p className="sem-resultados">
                                                    Nenhum aluno encontrado.
                                                </p>

                                            )}

                                        </section>
                                    )}

                                </div>

                            </div>

                            <div className="input">



                            </div>
                        </div>
                    </div>


                    <div className="boxD">
                        <div className="caixa-icons">
                            <img src="descriçao.svg" alt="" />

                            <h3>Descrição da Ocorrência</h3>
                        </div>
                        <textarea type="text" placeholder="Digite sobre o assunto ..." className="areaText" value={descricao || ""} onChange={(e) => setDescricao((e.target.value))} />
                    </div>

                    <div className="boxD">
                        <div className="caixa-icons">
                            <img src="aviao-papel.svg" alt="" />

                            <h3>Ações</h3>
                        </div>

                        <div className="areaAzul">
                            <button type="submit" className="btnAzul">Salvar e continuar</button>
                            <button type="button" className="btnBranco">Cancelar</button>
                        </div>
                    </div>
                </form>
            </div >
        </div >
    );
};

export default NovaOcorrencia;
