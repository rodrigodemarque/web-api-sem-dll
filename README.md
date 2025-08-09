# Web-API

API REST desenvolvida em C# utilizando .NET Framework 4.3, com integração ao banco de dados SQL Server Express. Esta API fornece endpoints para gerenciamento de dados de carros, permitindo operações CRUD (Create, Read, Update, Delete).

---

## Tecnologias Utilizadas

- C#  
- .NET Framework 4.3  
- SQL Server Express  
- Visual Studio 2022 (ambiente de desenvolvimento)

---

## Pré-requisitos

- .NET Framework 4.3 instalado  
- SQL Server Express configurado  
- Postman (ou outro cliente API) para testar os endpoints  
- Visual Studio 2022 para abrir e rodar a solução

---

## Como Executar

1. Clone o repositório:  
   ```bash
   git clone https://github.com/seu-usuario/web-api.git
2. Abra a solução no Visual Studio 2022.

3. Configure a string de conexão com o SQL Server Express no arquivo de configuração (ex: app.config ou web.config).

4. Execute o projeto (F5) para iniciar a API localmente.

5. Utilize o Postman para consumir os endpoints.

6. Endpoints Disponíveis
Método	Endpoint	Descrição
GET	/api/carros/	Retorna todos os carros
GET	/api/carros/{id}	Retorna carro pelo ID
GET	/api/carros/{nome}	Retorna carro pelo nome
POST	/api/carros/	Cria um novo carro
PUT	/api/carros/{id}	Atualiza um carro
DELETE	/api/carros/{id}	Remove um carro

Exemplo de Uso com Postman
Para obter todos os carros:
GET http://localhost:porta/api/carros/

Para buscar carro pelo ID:
GET http://localhost:porta/api/carros/5

Para criar um novo carro (Body em JSON):
{
  "nome": "Ford Mustang",
  "ano": 2020,
  "cor": "Vermelho"
}

Contribuição
Contribuições são bem-vindas! Para sugerir melhorias ou reportar bugs, abra uma issue ou envie um pull request.

Contato
Rodrigo Demarque
Email: rodrigodemarque@gmail.com
LinkedIn: https://www.linkedin.com/in/rodrigo-demarque/

Boa codificação!

