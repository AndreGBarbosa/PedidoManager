# 🛒 PedidoManager

Sistema simplificado de **Gerenciamento de Pedidos** desenvolvido por **Andre G. Barbosa** como parte do teste técnico para a vaga mencionada no documento da UXComex.

---

## 📌 Objetivo

Este projeto tem como finalidade demonstrar habilidades práticas em desenvolvimento fullstack utilizando a stack exigida:

- Backend: **C# com ASP.NET Core MVC**
- Banco de Dados: **SQL Server**
- ORM: **Dapper.NET**
- Frontend: **HTML5, CSS3, Bootstrap, jQuery**

---

## 🧱 Funcionalidades

### 👤 Gerenciamento de Clientes
- CRUD completo
- Campos: Nome, Email, Telefone, Data de Cadastro
- Filtro dinâmico por nome ou email

### 📦 Gerenciamento de Produtos
- CRUD completo
- Campos: Nome, Descrição, Preço, Quantidade em Estoque
- Filtro dinâmico por nome

### 🧾 Registro de Pedidos
- Seleção de cliente
- Adição de múltiplos produtos com quantidade
- Validação de estoque
- Cálculo automático do valor total
- Alteração de status (Novo, Processando, Finalizado)
- Listagem com filtro por cliente ou status
- Visualização de detalhes com itens

---

## 🧩 Arquitetura

- Separação em camadas: Controllers, Models, Repositories, ViewModels
- Padrão Repository com Dapper.NET
- Injeção de dependência nativa do ASP.NET Core
- Validações e tratamento básico de erros
- Código limpo, comentado e em inglês

---

## 🖥️ Interface

- Layout responsivo com Bootstrap
- Interações dinâmicas com jQuery (filtros, adição de itens sem refresh)
- Feedback visual com mensagens de sucesso

---

## 🧪 Testes

- Testes unitários básicos na camada de negócio (`PedidoServiceTests`)
- Validação de regras como cálculo de total e alteração de status

---

## 🗄️ Banco de Dados

Script SQL incluído para criação das tabelas:

- `Cliente`
- `Produto`
- `Pedido`
- `ItemPedido`

Inclui chaves estrangeiras e dados iniciais para testes.

---

## 🚀 Como executar

1. Clone o repositório:
   ```bash
   git clone https://github.com/seu-usuario/PedidoManager.git