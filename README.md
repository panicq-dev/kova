# Kova 💜

Uma aplicação web de conversas em tempo real feita para jogar e trocar ideia com os amigos — inspirada no Discord, mas construída do zero.

O projeto começou com o nome *Nexus*, mas acabei mudando para **Kova** por ser mais curto, direto e combinar melhor com a identidade que eu queria dar para a plataforma.

## Status

Em desenvolvimento. A base do projeto já está funcionando e sigo construindo os próximos recursos aos poucos.

## Tecnologias

* **Back-end:** C#, .NET, ASP.NET Core e Entity Framework Core
* **Tempo real e mensageria:** SignalR e RabbitMQ
* **Banco de dados:** PostgreSQL
* **Front-end:** Next.js, React, TypeScript e Tailwind CSS

## Como rodar

### Pré-requisitos
* [.NET SDK](https://dotnet.microsoft.com/)
* [Node.js](https://nodejs.org/)
* PostgreSQL

### Back-end
```bash
cd NexusChat.Api
dotnet restore
dotnet run
