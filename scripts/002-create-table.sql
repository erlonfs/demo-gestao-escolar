USE GestaoEscolar;
GO

DROP TABLE GES.SalaAluno;
DROP TABLE GES.Sala;
DROP TABLE GES.SalaTurno;
DROP TABLE GES.Escola;
DROP TABLE GES.Aluno;
DROP TABLE GES.AlunoSituacao;
DROP TABLE GES.PessoaFisica;

CREATE TABLE GES.PessoaFisica(
	Id INT IDENTITY NOT NULL,
	EntityId UNIQUEIDENTIFIER NOT NULL,
	DataCriacao DATETIME NOT NULL,
	Nome VARCHAR(400) NOT NULL,
	Cpf VARCHAR(11) NOT NULL,
	NomeSocial VARCHAR(400) NULL,
	Sexo CHAR(1) NOT NULL,
	DataNascimento DATETIME NOT NULL
)
GO
ALTER TABLE GES.PessoaFisica ADD CONSTRAINT PK_PessoaFisica PRIMARY KEY (Id)
GO
ALTER TABLE GES.PessoaFisica ADD CONSTRAINT UC_PessoaFisica_Cpf UNIQUE(Cpf);
GO

CREATE TABLE GES.AlunoSituacao (
	Id INT NOT NULL,
	Nome VARCHAR(400) NOT NULL
)
GO
ALTER TABLE GES.AlunoSituacao ADD CONSTRAINT PK_AlunoSituacao PRIMARY KEY (Id)
GO

CREATE TABLE GES.Aluno(
	Id INT IDENTITY NOT NULL,
	EntityId UNIQUEIDENTIFIER NOT NULL,
	DataCriacao DATETIME NOT NULL,
	PessoaFisicaId INT NOT NULL,
	ResponsavelId INT NOT NULL,
	Matricula INT NOT NULL,
	SituacaoId INT NOT NULL,
)
GO
ALTER TABLE GES.Aluno ADD CONSTRAINT PK_Aluno PRIMARY KEY (Id)
GO
ALTER TABLE GES.Aluno ADD CONSTRAINT UC_Aluno_Matricula UNIQUE(Matricula);
GO
ALTER TABLE GES.Aluno ADD CONSTRAINT FK_Aluno_PessoaFisica FOREIGN KEY (PessoaFisicaId) REFERENCES GES.PessoaFisica(Id)
GO
ALTER TABLE GES.Aluno ADD CONSTRAINT FK_Aluno_Responsavel FOREIGN KEY (ResponsavelId) REFERENCES GES.PessoaFisica(Id)
GO
ALTER TABLE GES.Aluno ADD CONSTRAINT FK_Aluno_Situacao FOREIGN KEY (SituacaoId) REFERENCES GES.AlunoSituacao(Id)
GO

CREATE TABLE GES.Escola (
	Id INT IDENTITY NOT NULL,
	EntityId UNIQUEIDENTIFIER NOT NULL,
	DataCriacao DATETIME NOT NULL,
	Nome VARCHAR(400) NOT NULL
)
GO
ALTER TABLE GES.Escola ADD CONSTRAINT PK_Escola PRIMARY KEY (Id)
GO

CREATE TABLE GES.SalaTurno (
	Id INT NOT NULL,
	Nome VARCHAR(400) NOT NULL
)
GO
ALTER TABLE GES.SalaTurno ADD CONSTRAINT PK_SalaTurno PRIMARY KEY (Id)
GO

CREATE TABLE GES.Sala (
	Id INT IDENTITY NOT NULL,
	EntityId UNIQUEIDENTIFIER NOT NULL,
	EscolaId INT NOT NULL,
	FaseAno VARCHAR(100) NOT NULL,
	TurnoId INT NOT NULL
)
GO
ALTER TABLE GES.Sala ADD CONSTRAINT PK_Sala PRIMARY KEY (Id)
GO
ALTER TABLE GES.Sala ADD CONSTRAINT FK_Sala_Escola FOREIGN KEY (EscolaId) REFERENCES GES.Escola(Id)
GO
ALTER TABLE GES.Sala ADD CONSTRAINT FK_Sala_Turno FOREIGN KEY (TurnoId) REFERENCES GES.SalaTurno(Id)
GO

CREATE TABLE GES.SalaAluno (
	Id INT IDENTITY NOT NULL,
	SalaId INT NOT NULL,
	AlunoId INT NOT NULL
)
GO
ALTER TABLE GES.SalaAluno ADD CONSTRAINT PK_SalaAluno PRIMARY KEY (Id)
GO
ALTER TABLE GES.SalaAluno ADD CONSTRAINT FK_SalaAluno_Sala FOREIGN KEY (SalaId) REFERENCES GES.Sala(Id)
GO
ALTER TABLE GES.SalaAluno ADD CONSTRAINT FK_SalaAluno_Aluno FOREIGN KEY (AlunoId) REFERENCES GES.Aluno(Id)
GO
ALTER TABLE GES.SalaAluno ADD CONSTRAINT UC_SalaAluno_Sala_Aluno UNIQUE(SalaId, AlunoId);
GO
