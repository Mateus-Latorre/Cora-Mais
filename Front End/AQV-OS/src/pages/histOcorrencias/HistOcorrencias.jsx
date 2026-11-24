import { useContext, useEffect, useState } from "react";
import Menu from "../../components/menu/menu";
import "./HistOcorrencias.css";
import { Link } from "react-router-dom";
import { UsuarioContext } from "../../context/UsuarioContext/UsuarioContext";
import { AQVContext } from "../../context/AQVContext/AQVContext";

const HistOcorrencias = () => {
  const { usuario } = useContext(UsuarioContext);
  const { listagemOcorrencia, getOcorrencia } = useContext(AQVContext);

  const [busca, setBusca] = useState("");
  const [alunoSelecionado, setAlunoSelecionado] = useState(null);
  const [filtroStatus, setFiltroStatus] = useState("");
  const [filtroData, setFiltroData] = useState("");

  // Lista de ocorrências
  const ocorrencias = listagemOcorrencia || [];

  // ==========================================
  // FORMATAR DATA
  // ==========================================

  const formatarData = (data) => {
    if (!data) return "Não informado";

    return String(data).slice(0, 10).split("-").reverse().join("/");
  };

  // ==========================================
  // IDENTIFICADOR DO ALUNO
  // ==========================================

  const obterIdAluno = (aluno) => aluno?.idAluno ?? aluno?.cpf ?? aluno?.nome;

  // ==========================================
  // LISTA DE ALUNOS SEM REPETIÇÃO
  // ==========================================

  const listaAlunos = [
    ...new Map(
      ocorrencias
        .filter((o) => o?.idAlunoNavigation != null)
        .map((o) => [obterIdAluno(o.idAlunoNavigation), o.idAlunoNavigation]),
    ).values(),
  ];

  // ==========================================
  // PESQUISA DE ALUNO POR NOME OU CPF
  // ==========================================

  const alunosEncontrados = listaAlunos.filter((aluno) => {
    const nome = String(aluno?.nome || "").toLowerCase();
    const cpf = String(aluno?.cpf || "").replace(/\D/g, "");

    const pesquisa = busca.trim().toLowerCase();
    const pesquisaCPF = busca.replace(/\D/g, "");

    return (
      aluno &&
      pesquisa.length > 0 &&
      (nome.includes(pesquisa) ||
        (pesquisaCPF.length > 0 && cpf.includes(pesquisaCPF)))
    );
  });

  // ==========================================
  // APLICAÇÃO DOS FILTROS
  // ==========================================

  // Aplicação dos três filtros
  const ocorrenciasFiltradas = ocorrencias.filter((ocorrencia) => {
    // Filtro por aluno
    const aluno = ocorrencia.idAlunoNavigation;

    const correspondeAluno =
      !alunoSelecionado ||
      obterIdAluno(aluno) === obterIdAluno(alunoSelecionado);

    // Filtro por status
    const correspondeStatus =
      !filtroStatus || ocorrencia.idOstatus === filtroStatus;

    // Filtro por data de abertura
    const dataOcorrencia = ocorrencia.dataAbertura
      ? String(ocorrencia.dataAbertura).slice(0, 10)
      : "";

    const correspondeData = !filtroData || dataOcorrencia === filtroData;

    return correspondeAluno && correspondeStatus && correspondeData;
  });

  // ==========================================
  // CONTAGEM DOS CARDS
  // ==========================================

  const abertas = ocorrenciasFiltradas.filter(
    (o) => o.idOstatus === "ABERTA",
  ).length;

  const andamento = ocorrenciasFiltradas.filter(
    (o) => o.idOstatus === "ANDAMENTO",
  ).length;

  const finalizadas = ocorrenciasFiltradas.filter(
    (o) => o.idOstatus === "FINALIZADA",
  ).length;

  const feedbacks = ocorrenciasFiltradas.filter(
    (o) => o.idOstatus === "FEEDBACK",
  ).length;

  // Total de ocorrências após a aplicação dos filtros
  const total = ocorrenciasFiltradas.length;

  // ==========================================
  // CARREGAR OCORRÊNCIAS
  // ==========================================

  useEffect(() => {
    if (usuario?.idUsuario) {
      getOcorrencia(usuario.idUsuario);
    }
  }, [usuario?.idUsuario]);

  // ==========================================
  // TELA
  // ==========================================

  return (
    <div className="fundo-pagina dashboard-body">
      <Menu />

      <main className="container-ocorrencias">
        {/* TÍTULO E BOTÃO */}

        <div className="div-Botao">
          <h1>Ocorrências</h1>

          {usuario?.perfil === "Professor" && (
            <Link to="/CadastroOcorrencia" className="button-criar">
              + Nova ocorrência
            </Link>
          )}
        </div>

        {/* CARDS DE STATUS */}

        <div className="etapas-ocorrencia">
          {/* TOTAL */}

          <article className="card">
            <div className="caxa">
              <div className="indicador total"></div>
              <h4>Total</h4>
            </div>
            <p>{total}</p>
          </article>

          {/* ABERTAS */}

          <article className="card">
            <div className="caxa">
              <div className="indicador abertas"></div>
              <h4>Abertas</h4>
            </div>
            <p>{abertas}</p>
          </article>

          {/* EM ANDAMENTO */}

          <article className="card">
            <div className="caxa">
              <div className="indicador EmAndamento"></div>
              <h4>Em andamento</h4>
            </div>
            <p>{andamento}</p>
          </article>

          {/* FINALIZADAS */}

          <article className="card">
            <div className="caxa">
              <div className="indicador finalizadas"></div>
              <h4>Finalizadas</h4>
            </div>
            <p>{finalizadas}</p>
          </article>

          {/* FEEDBACK */}

          <article className="card">
            <div className="caxa">
              <div className="indicador feedback"></div>
              <h4>Feedback</h4>
            </div>
            <p>{feedbacks}</p>
          </article>
        </div>

        {/* PESQUISA E FILTROS */}

        <div className="filtros-ocorrencias">
          {/* BUSCA POR ALUNO */}

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
              }}
            />

            {/* RESULTADOS DA BUSCA */}

            {!alunoSelecionado && busca.trim() !== "" && (
              <div className="resultado-busca">
                {alunosEncontrados.length > 0 ? (
                  alunosEncontrados.map((aluno) => (
                    <button
                      type="button"
                      key={obterIdAluno(aluno)}
                      className="opcao-aluno"
                      onClick={() => {
                        setAlunoSelecionado(aluno);
                        setBusca(aluno.nome);
                      }}
                    >
                      <strong>{aluno.nome}</strong>
                      <span>CPF: {aluno.cpf}</span>
                    </button>
                  ))
                ) : (
                  <p className="sem-resultados">Nenhum aluno encontrado.</p>
                )}
              </div>
            )}
          </div>

          {/* FILTRO DE STATUS */}

          <div className="input-wrapper">
            <select
              value={filtroStatus}
              onChange={(e) => setFiltroStatus(e.target.value)}
            >
              <option value="">Todos os status</option>
              <option value="ABERTA">Aberta</option>
              <option value="ANDAMENTO">Em andamento</option>
              <option value="FINALIZADA">Finalizada</option>
              <option value="FEEDBACK">Feedback</option>
            </select>
          </div>

          {/* FILTRO DE DATA */}

          <input
            type="date"
            value={filtroData}
            onChange={(e) => setFiltroData(e.target.value)}
          />
        </div>

        {/* TABELA */}

        <div className="tabela-cabecalho">
          <span>OCORRÊNCIAS</span>
          <span>PROFESSOR</span>
          <span>DATA</span>
          <span>STATUS</span>
        </div>

        <div className="tabela-conteudo">
          {ocorrenciasFiltradas.length > 0 ? (
            ocorrenciasFiltradas.map((ocorrencia) => (
              <Link
                key={ocorrencia.idOcorrencia}
                to={`/Ocorrencias/${ocorrencia.idOcorrencia}`}
                className="informacoes-ocorrencia"
              >
                {/* OCORRÊNCIA E ALUNO */}

                <div className="descri">
                  <strong>{ocorrencia.titulo}</strong>

                  <p>
                    {ocorrencia.idAlunoNavigation?.nome ||
                      "Aluno não informado"}
                  </p>
                </div>

                {/* PROFESSOR */}

                <div className="prof">
                  {ocorrencia.idUsuarioNavigation?.nome || "Não informado"}
                  {console.log("ocorrencia.idUsuarioNavigation?.nome", ocorrencia.idUsuarioNavigation?.nome)}
                </div>

                {/* DATA */}

                <div className="data">
                  {formatarData(ocorrencia.dataAbertura)}
                </div>

                {/* STATUS */}

                <div
                  className={`status ${
                    ocorrencia.idOstatus === "ABERTA"
                      ? "aberta"
                      : ocorrencia.idOstatus === "ANDAMENTO"
                        ? "andamento"
                        : ocorrencia.idOstatus === "FINALIZADA"
                          ? "finalizada"
                          : ocorrencia.idOstatus === "FEEDBACK"
                            ? "feedback"
                            : ""
                  }`}
                >
                  <span></span>

                  <strong>
                    {ocorrencia.idOstatus === "ABERTA"
                      ? "Aberta"
                      : ocorrencia.idOstatus === "ANDAMENTO"
                        ? "Em andamento"
                        : ocorrencia.idOstatus === "FINALIZADA"
                          ? "Finalizada"
                          : ocorrencia.idOstatus === "FEEDBACK"
                            ? "Feedback"
                            : "Sem status"}
                  </strong>
                </div>
              </Link>
            ))
          ) : (
            <p className="sem-resultados">Nenhuma ocorrência encontrada.</p>
          )}
        </div>
      </main>
    </div>
  );
};

export default HistOcorrencias;
