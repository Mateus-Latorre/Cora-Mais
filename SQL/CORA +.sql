CREATE DATABASE CORA;
USE CORA;
USE master;
GO
CREATE DATABASE CORA
ON (FILENAME = 'C:\Users\50055487858\CORA.mdf'),
   (FILENAME = 'C:\Users\50055487858\CORA_log.ldf')
FOR ATTACH;
GO
CREATE TABLE TipoUsuario(
IdTipoUsuario UNIQUEIDENTIFIER PRIMARY KEY DEFAULT ((NEWID())),
Titulo VARCHAR(100) NOT NULL,
)
SELECT * FROM TipoUsuario;
CREATE TABLE Usuario (
IdUsuario UNIQUEIDENTIFIER PRIMARY KEY DEFAULT ((NEWID())),
Nome VARCHAR(200) NOT NULL,
Email VARCHAR(256) UNIQUE NOT NULL,
Senha VARCHAR (60) NOT NULL,
Numero VARCHAR (11) UNIQUE NOT NULL,

IdTipoUsuario UNIQUEIDENTIFIER FOREIGN KEY REFERENCES TipoUsuario(IdTipoUsuario)
)
SELECT * FROM Usuario;
CREATE TABLE Aluno (
IdAluno UNIQUEIDENTIFIER PRIMARY KEY DEFAULT ((NEWID())),
Nome VARCHAR(200) NOT NULL,
Email VARCHAR(256) UNIQUE NOT NULL,
CPF VARCHAR(11) UNIQUE NOT NULL,
Numero VARCHAR (11) UNIQUE NOT NULL,
)
SELECT * FROM Aluno
CREATE TABLE Turma (
IdTurma UNIQUEIDENTIFIER PRIMARY KEY DEFAULT ((NEWID())),
Nome VARCHAR(200) NOT NULL,
)
SELECT * FROM Turma;

CREATE TABLE Responsavel (
IdResponsavel UNIQUEIDENTIFIER PRIMARY KEY DEFAULT ((NEWID())),
Nome VARCHAR(200) NOT NULL,
Email VARCHAR(256) UNIQUE NOT NULL,
Numero VARCHAR (11) UNIQUE NOT NULL,

IdAluno UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Aluno(IdAluno)
)
SELECT * FROM Responsavel

CREATE TABLE Ostatus (
IdOstatus VARCHAR(20) PRIMARY KEY,
Titulo VARCHAR(50) NOT NULL,
)
SELECT * FROM Ostatus

CREATE TABLE Feedback (
IdFeedback UNIQUEIDENTIFIER PRIMARY KEY DEFAULT ((NEWID())),
Mensagem VARCHAR(300) NOT NULL,
DataEnvio DATETIME DEFAULT CURRENT_TIMESTAMP,
Lido BIT DEFAULT 0,
Anexo VARCHAR(255),

IdOcorrencia UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Ocorrencia(IdOcorrencia)
)
SELECT*FROM Feedback


CREATE TABLE Ocorrencia (
IdOcorrencia  UNIQUEIDENTIFIER PRIMARY KEY DEFAULT ((NEWID())),
Titulo VARCHAR(55) NOT NULL,
Descricao VARCHAR(250) NOT NULL,
DataAbertura DATETIME DEFAULT CURRENT_TIMESTAMP,

IdAluno UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Aluno(IdAluno),
IdOstatus VARCHAR(20) FOREIGN KEY REFERENCES Ostatus(IdOstatus),
IdUsuario UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Usuario(IdUsuario),
)

SELECT*FROM Ocorrencia

--alteração/correção das tabela--
SELECT *
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE';

ALTER TABLE Aluno
ADD IdTurma UNIQUEIDENTIFIER NULL;

ALTER TABLE Aluno
ADD CONSTRAINT FK_Aluno_Turma
FOREIGN KEY (IdTurma) REFERENCES Turma(IdTurma);

ALTER TABLE Ocorrencia
ADD Prioridade VARCHAR(20) NOT NULL;

ALTER TABLE Ocorrencia
ADD Anexo VARCHAR(200) NULL;

ALTER TABLE Ocorrencia
ADD CONSTRAINT CK_Ocorrencia_Prioridade
CHECK (Prioridade IN ('Urgente', 'Importante', 'Rotina'));

 ALTER TABLE Ocorrencia ADD Anexo VARCHAR(255) NULL;

CREATE TABLE Encaminhamento (
    IdEncaminhamento UNIQUEIDENTIFIER PRIMARY KEY DEFAULT (NEWID()),
    IdOcorrencia UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT FK_Encaminhamento_Ocorrencia REFERENCES Ocorrencia(IdOcorrencia),
    -- um dos dois é preenchido: responsável do aluno OU coordenador
    IdResponsavel UNIQUEIDENTIFIER NULL
        CONSTRAINT FK_Encaminhamento_Responsavel REFERENCES Responsavel(IdResponsavel),
    IdUsuarioCoordenador UNIQUEIDENTIFIER NULL
        CONSTRAINT FK_Encaminhamento_Coordenador REFERENCES Usuario(IdUsuario),
    IdUsuarioAQV UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT FK_Encaminhamento_AQV REFERENCES Usuario(IdUsuario),
    DataEnvio DATETIME NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    DataDevolutiva DATETIME NULL,
    Observacao VARCHAR(300) NULL,

    CONSTRAINT CK_Encaminhamento_Destino
        CHECK ((IdResponsavel IS NOT NULL AND IdUsuarioCoordenador IS NULL)
            OR (IdResponsavel IS NULL AND IdUsuarioCoordenador IS NOT NULL))
);


ALTER TABLE Ocorrencia
ADD IdResponsavelAtual UNIQUEIDENTIFIER NULL
    CONSTRAINT FK_Ocorrencia_ResponsavelAtual REFERENCES Responsavel(IdResponsavel),
    IdCoordenadorAtual UNIQUEIDENTIFIER NULL
    CONSTRAINT FK_Ocorrencia_CoordenadorAtual REFERENCES Usuario(IdUsuario);


    insert into TipoUsuario(Titulo)
    values('AQV')

    insert into Usuario(Nome, Email, Senha, Numero, IdTipoUsuario)
    values('Mateus','mateus@gmail.com','123','11912345678','4FDEE162-F5C4-4C4F-8615-49E550246F37')

    USE CORA;

INSERT INTO Ostatus (IdOstatus, Titulo) VALUES ('ABERTA', 'Aberta');
INSERT INTO Ostatus (IdOstatus, Titulo) VALUES ('ANDAMENTO', 'Em andamento');
INSERT INTO Ostatus (IdOstatus, Titulo) VALUES ('ENCAMINHADA', 'Encaminhada');
INSERT INTO Ostatus (IdOstatus, Titulo) VALUES ('AGUARDANDO_FEEDBACK', 'Aguardando feedback');
INSERT INTO Ostatus (IdOstatus, Titulo) VALUES ('FINALIZADA', 'Finalizada');

USE CORA;
-- senha nova = "123456" para o usuário abaixo (troque o e-mail)
USE CORA;

UPDATE Usuario
SET Senha = '$2b$11$6fyx/gExENXjdpVGgKy6geMvCdRs0Kamcg6Gjr3IFqyE8dmP9c4/2'
WHERE Email = 'ste@gmail.com';