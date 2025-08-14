-- 1. Criar o banco de dados
IF DB_ID('PedidoDB') IS NULL
    CREATE DATABASE PedidoDB;
GO

-- 2. Usar o banco de dados
USE PedidoDB;
GO

-- 3. Criar tabelas

-- CLIENTE
CREATE TABLE Cliente (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nome NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Telefone NVARCHAR(20),
    DataCadastro DATE NOT NULL
);

-- PRODUTO
CREATE TABLE Produto (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nome NVARCHAR(100) NOT NULL,
    Descricao NVARCHAR(255),
    Preco DECIMAL(10,2) NOT NULL,
    QuantidadeEstoque INT NOT NULL
);

-- PEDIDO
CREATE TABLE Pedido (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ClienteId INT NOT NULL,
    Data DATE NOT NULL,
    ValorTotal DECIMAL(10,2) NOT NULL,
    Status NVARCHAR(20) NOT NULL CHECK (Status IN ('Novo', 'Processando', 'Finalizado')),
    FOREIGN KEY (ClienteId) REFERENCES Cliente(Id)
);

-- ITEM DO PEDIDO
CREATE TABLE ItemPedido (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PedidoId INT NOT NULL,
    ProdutoId INT NOT NULL,
    Quantidade INT NOT NULL,
    PrecoUnitario DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (PedidoId) REFERENCES Pedido(Id),
    FOREIGN KEY (ProdutoId) REFERENCES Produto(Id)
);

-- ALTERAÇÃO DE STATUS DO PEDIDO
CREATE TABLE AlteracaoStatusPedido (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PedidoId INT NOT NULL,
    StatusAnterior NVARCHAR(20),
    StatusNovo NVARCHAR(20),
    DataAlteracao DATETIME NOT NULL,
    FOREIGN KEY (PedidoId) REFERENCES Pedido(Id)
);

-- 4. Inserir dados iniciais

-- CLIENTES
INSERT INTO Cliente (Nome, Email, Telefone, DataCadastro) VALUES
('Andre Silva', 'andre@email.com', '1199998888', GETDATE()),
('Fernanda Costa', 'fernanda@email.com', '1198888777', GETDATE());

-- PRODUTOS
INSERT INTO Produto (Nome, Descricao, Preco, QuantidadeEstoque) VALUES
('Teclado Gamer', 'Teclado RGB com mecânica azul', 250.00, 10),
('Mouse Sem Fio', 'Mouse óptico com conexão 2.4GHz', 120.00, 15),
('Headset Bluetooth', 'Fone com microfone e som estéreo', 299.00, 8);