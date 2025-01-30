
# **Digital Wallet API**

A **Digital Wallet API** é uma aplicação para gerenciar carteiras digitais, transações financeiras e usuários. A API oferece funcionalidades como registro de usuários, gerenciamento de carteiras, depósitos, saques, transferências e consulta de histórico de transações.

## **Tecnologias Utilizadas**

A API foi desenvolvida utilizando as seguintes tecnologias e ferramentas:

- **ASP.NET Core 8.0**: Framework principal para construção da API.
- **Entity Framework Core**: ORM para manipulação do banco de dados.
- **PostgreSQL**: Banco de dados relacional utilizado.
- **JWT (JSON Web Token)**: Para autenticação e autorização.
- **xUnit**: Framework para testes unitários.
- **Moq**: Biblioteca para criação de mocks nos testes.
- **Serilog**: Para logging estruturado.

## **Configuração do Ambiente**

### **Pré-requisitos**
Certifique-se de ter instalado:
- **[.NET SDK 8.0+](https://dotnet.microsoft.com/download/dotnet/8.0)**
- **[PostgreSQL](https://www.postgresql.org/)**
- **Uma ferramenta para testar requisições HTTP, como [Postman](https://www.postman.com/) ou [Insomnia](https://insomnia.rest/).**

### **Configuração**
1. Clone este repositório:
git clone https://github.com/seu-repositorio/digital-wallet-api.git
cd digital-wallet-api

2. Configure o banco de dados no arquivo `appsettings.json`:
"ConnectionStrings": {
"DefaultConnection": "Host=localhost;Database=walletdb;Username=postgres;Password=sua_senha"
}

3. Execute a aplicação:
dotnet run ou rodar via docker.


5. Acesse a documentação Swagger em: http://localhost:44390/swagger  ou a porta que será especificada no lauchSettings.json ou que será mapeada para rodar no container docker.


### **Uso**

Agora você pode testar os endpoints de API disponíveis usando ferramentas como Postman ou Insomnia.

Aqui alguns explos de endpoints:

### Auth
- **POST** `/api/Auth/login` - Autenticação de usuários.

### Transactions
- **POST** `/api/Transactions/deposit` - Realiza um depósito na conta do usuário.
- **POST** `/api/Transactions/withdraw` - Realiza um saque da conta do usuário.
- **POST** `/api/Transactions/transfer` - Realiza uma transferência entre contas.
- **GET** `/api/Transactions/transfers` - Lista as transferências realizadas.

### Users
- **POST** `/api/Users/register` - Registra um novo usuário.
- **GET** `/api/Users` - Lista os usuários cadastrados.

### Wallet
- **GET** `/api/Wallet/balance` - Obtém o saldo da carteira do usuário.


### **Contribuindo**

Contribuições são sempre bem-vindas! Sinta-se à vontade para bifurcar o repositório e enviar um pull request com melhorias ou novos recursos que desejar adicionar.

Este projeto foi desenvolvido para fins de estudo e prática de padrões e boas práticas de programação. Fique à vontade para utilizar o código ou contribuir da maneira que preferir! É um projeto em evolução.

### **Licença**
O código neste projeto está disponível gratuitamente para qualquer um usar, modificar e distribuir. Não há restrições quanto ao seu uso; apenas certifique-se de dar os devidos créditos.

### **Contato**
Para quaisquer dúvidas ou sugestões, entre em contato com carlos.costajunior@hotmail.com
