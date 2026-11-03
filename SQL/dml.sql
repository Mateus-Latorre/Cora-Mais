use CORA;

INSERT INTO Usuario(Nome, Email, Senha, Numero, IdTipoUsuario)
VALUES('Isabella Moraes', 'bella@gmail.com', '1234','', 'A1FD7A90-0692-4448-8A22-3D02805A7FBE');


SELECT * FROM Usuario

INSERT INTO TipoUsuario(Titulo)
VALUES('AQV');


SELECT * FROM TipoUsuario


SELECT
    Email,
    LEN(Senha) AS TamanhoSenha
FROM Usuario;