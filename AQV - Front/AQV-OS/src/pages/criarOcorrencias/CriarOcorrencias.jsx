import { useContext, useEffect, useState } from "react";
import "./CriarOcorrencias.css"
import Menu from "../../components/menu/menu";
import { useNavigate } from "react-router-dom";
import { UsuarioContext } from "../../context/UsuarioContext/UsuarioContext";
import api from "../../services/Services";

const NovaOcorrencia = () => {

    const [visibilidade, setVisibilidade] = useState(false);
    const [titulo, setTitulo] = useState("");
    const [listaAluno, setListaAluno] = useState([]);
    const [arrFiltrados, setArrFiltrados] = useState([]);
    const [IdAluno, setIdAluno] = useState("");
    const [nomeAluno, setNomeAluno] = useState("");
    const [turma, setTurma] = useState("");
    const [descricao, setDescricao] = useState("");
    const { setUsuario } = useContext(UsuarioContext);

    const getAlunos = async () => {
        try {
            debugger;
            const retornoAPI = await api.get("/Aluno");

            console.log(retornoAPI.data);

            setListaAluno(retornoAPI.data);
        } catch (error) {
            console.log(error);

            Alerta({
                title: "Erro",
                text: "Erro ao buscar gêneros!",
                icon: "error",
            });
        }
    };

    const cadastrarOcorrencia = async () => {
        try {

            debugger;

            const token = localStorage.getItem("token");

            const ocorrenciaDados = {
                titulo: titulo,
                descricao: descricao,
                idAluno: aluno.IdAluno,
                prioridade: prioridade,
                anexo: null
            };

            const resposta = await api.post("/api/Ocorrencia", ocorrenciaDados,
                {
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                }
            );

            alert("Ocorrência cadastrada com sucesso!");

            setTitulo("");
            setDescricao("");
            setAlunoSelecionado(null);
            setPrioridade("");

        } catch (erro) {
            console.error(
                "Erro:",
                erro.response?.data || erro.message
            );

            alert("Erro ao cadastrar ocorrência!");
        }
    };




    // filtrar alunos
    const filtrarAlunos = (txtAluno) => {

        const procurados = listaAluno.filter((aluno) => {
            return aluno.nome.toLowerCase().startsWith(txtAluno.toLowerCase())
        })

        setArrFiltrados(procurados)

        console.log(procurados);

    }
    useEffect(() => {
        getAlunos();
    }, []);



    return (
        <div className="body">

            { /* *******           Menu           ********** */}

            <Menu></Menu>

            {/* **************        Tela          ***************** */}
            <div className="div">
                <div className="titulo">
                    <p>Nova Ocorrência</p>
                    <h1>Registrar uma nova Ocorrência</h1>
                </div>
                <form action="submit" >
                    <div className="box">
                        <div className="caixa-icons">
                            <img src="identificacao.svg" alt="" />

                            <h3>Identificação da Ocorrência</h3>
                        </div>
                        <div className="inputs">
                            <div className="input">
                                <h5>Titulo da Ocorrência</h5>

                                <input type="text" placeholder="Ex: Atraso freuqentenas entregas das atividades" value={titulo} onChange={(e) => setTitulo(e.target.value)} />
                            </div>

                            <div className="input">
                                <h5>Prioridade</h5>
                                <select
                                    name="genero"
                                    onChange={(e) => props.setPrioridade(e.target.value)}
                                >
                                    <option value="" disabled>
                                        Selecione
                                    </option>

                                    <option value="Rotina">
                                        Rotina
                                    </option>
                                    <option value="Importante">
                                        Importante
                                    </option>
                                    <option value="Urgente">
                                        Urgente
                                    </option>
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
                                    <i className="fa-solid fa-user icon"><img src="lupa.svg" alt="" /></i>
                                    <input type="text" placeholder="Buscar por CPF ou nome"

                                        onKeyUp={
                                            (e) => filtrarAlunos(e.target.value)
                                        }

                                        onFocus={() => setVisibilidade(true)} onBlur={() => setVisibilidade(false)} />
                                    {visibilidade && (
                                        <section className="list-alu">
                                            {
                                                arrFiltrados.length == 0 ? listaAluno.map((alu) => {
                                                    return <p className="alu" key={alu.IdAluno} value={alu.IdAluno} onClick={() => {
                                                        setIdAluno(alu.IdAluno)
                                                        setNomeAluno(alu.nome)
                                                    }}>
                                                        {alu.nome}
                                                    </p>
                                                })
                                                    :

                                                    arrFiltrados.map((alu) => {
                                                        return <p className="alu" key={alu.IdAluno} value={alu.IdAluno} onClick={() => {
                                                            setIdAluno(alu.IdAluno)
                                                            setNomeAluno(alu.nome)
                                                        }}>
                                                            {alu.nome}
                                                        </p>
                                                    })
                                            }


                                        </section>

                                    )}


                                </div>

                            </div>

                            <div className="input">
                                <h5>Turma</h5>

                                <input type="" placeholder="Selecione a Turma" value={turma} onChange={(e) => setTurma(e.target.value)} />
                            </div>
                        </div>
                    </div>


                    <div className="boxD">
                        <div className="caixa-icons">
                            <img src="descriçao.svg" alt="" />

                            <h3>Descrição da Ocorrência</h3>
                        </div>
                        <textarea type="text" placeholder="Digite sobre o assunto ..." className="areaText" />
                    </div>

                    <div className="boxD">
                        <div className="caixa-icons">
                            <img src="aviao-papel.svg" alt="" />

                            <h3>Ações</h3>
                        </div>

                        <div className="areaAzul">
                            <button onClick={cadastrarOcorrencia} className="btnAzul">Salvar e continuar</button>
                            <button className="btnBranco">Cancelar</button>
                        </div>
                    </div>
                </form>
            </div >
        </div >
    );
};

export default NovaOcorrencia;
