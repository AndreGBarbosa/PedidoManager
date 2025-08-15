IF DB_ID('PedidoDB') IS NULL
    CREATE DATABASE PedidoDB;
GO

USE PedidoDB;
GO

CREATE TABLE Cliente (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nome NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Telefone NVARCHAR(20),
    DataCadastro DATE NOT NULL
);

CREATE TABLE Produto (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nome NVARCHAR(100) NOT NULL,
    Descricao NVARCHAR(255),
    Preco DECIMAL(10,2) NOT NULL,
    QuantidadeEstoque INT NOT NULL,
    Ativo BIT NOT NULL DEFAULT 1 
);

CREATE TABLE Pedido (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ClienteId INT NOT NULL,
    DataPedido DATE NOT NULL,
    ValorTotal DECIMAL(10,2) NOT NULL,
    Status NVARCHAR(20) NOT NULL CHECK (Status IN ('Novo', 'Processando', 'Finalizado')),
    FOREIGN KEY (ClienteId) REFERENCES Cliente(Id)
);

CREATE TABLE ItemPedido (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PedidoId INT NOT NULL,
    ProdutoId INT NOT NULL,
    Quantidade INT NOT NULL,
    PrecoUnitario DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (PedidoId) REFERENCES Pedido(Id),
    FOREIGN KEY (ProdutoId) REFERENCES Produto(Id)
);

CREATE TABLE AlteracaoStatusPedido (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PedidoId INT NOT NULL,
    StatusAnterior NVARCHAR(20),
    StatusNovo NVARCHAR(20),
    DataAlteracao DATETIME NOT NULL,
    FOREIGN KEY (PedidoId) REFERENCES Pedido(Id)
);

INSERT INTO Cliente (Nome, Email, Telefone, DataCadastro) VALUES
('Andre Silva', 'andre@email.com', '1199998888', GETDATE()),
('Fernanda Costa', 'fernanda@email.com', '1198888777', GETDATE());

INSERT INTO Produto (Nome, Descricao, Preco, QuantidadeEstoque, Ativo) VALUES
('Teclado Gamer', 'Teclado RGB com mecânica azul', 250.00, 10, 1),
('Mouse Sem Fio', 'Mouse óptico com conexão 2.4GHz', 120.00, 15, 1),
('Headset Bluetooth', 'Fone com microfone e som estéreo', 299.00, 8, 1);

INSERT INTO Pedido (ClienteId, DataPedido, ValorTotal, Status) VALUES
(1, GETDATE(), 370.00, 'Novo'),
(2, GETDATE(), 299.00, 'Processando');

INSERT INTO ItemPedido (PedidoId, ProdutoId, Quantidade, PrecoUnitario) VALUES
(1, 1, 1, 250.00),
(1, 2, 1, 120.00);

INSERT INTO ItemPedido (PedidoId, ProdutoId, Quantidade, PrecoUnitario) VALUES
(2, 3, 1, 299.00);