# Paynext

Paynext é uma solução completa para gestão de contratos, solicitações de antecipação e administração de usuários, composta por um backend robusto em .NET e um frontend moderno em React.

## Demonstração Online

- **API (Swagger):** [https://paynext-api.fekler.tec.br/swagger/index.html](https://paynext-api.fekler.tec.br/swagger/index.html)
- **App:** [https://paynext.fekler.tec.br/login](https://paynext.fekler.tec.br/login)

### Logins de Demonstração
- **Administrador:**
  - Email: `admin@email.com`
  - Senha: `Abcd1234!@#$`
- **Cliente:**
  - Email: `cliente@email.com`
  - Senha: `Abcd1234!@#$`

---

## Tecnologias Utilizadas

### Frontend
- **React**: Biblioteca principal para construção da interface
- **Material UI (MUI v6+)**: Componentes visuais modernos e responsivos
- **Tailwind CSS**: Utilitário para estilização rápida e customizável
- **Axios**: Cliente HTTP para comunicação com a API
- **Redux**: Gerenciamento de estado global
- **Vite**: Ferramenta de build e desenvolvimento rápido

### Backend
- **.NET 8+**: Framework para construção de APIs robustas e performáticas
- **Entity Framework Core**: ORM para acesso a dados
- **JWT**: Autenticação baseada em tokens
- **BCrypt**: Hash de senhas seguro
- **PostgreSQL**: Banco de dados utilizado

## Arquitetura

O projeto segue os princípios da **Clean Architecture**, utilizando os padrões **Repository** e **Result** para garantir separação de responsabilidades, testabilidade e manutenção facilitada.

```
Paynext/
  ├── src/
  │   ├── Backend/         # Solução .NET (API, Application, Domain, Infra, IOC)
  │   └── Frontend/        # Aplicação React (src, public, etc)
  ├── SharedKernel/        # Componentes compartilhados .NET
  ├── README.md            # Este arquivo
  └── ...
```

## Pré-requisitos
- **Node.js** >= 16.x
- **.NET SDK** >= 8.0
- **Docker** (opcional, para rodar containers)

## Instalação e Uso

### 1. Clonar o repositório
```bash
# HTTPS
git clone https://github.com/seu-usuario/paynext.git
cd paynext
```

### 2. Configurar variáveis de ambiente (.env)
Crie um arquivo `.env` na pasta raiz do projeto com as seguintes variáveis:
```
DATABASE_URL= # String de conexão com o banco PostgreSQL
TOKEN_JWT_SECRET= # Segredo para geração dos tokens JWT
TOKEN_JWT_LIFETIME_MINUTES= # Tempo de expiração do token (em minutos)
API_URL= # URL base da API
APP_URL= # URL base do App
```

### 3. Rodar o Backend (.NET)
```bash
cd src/Backend/Paynext.API
# Restaurar pacotes
dotnet restore
# Rodar migrações (opcional, se usar banco local)
dotnet ef database update
# Iniciar API
dotnet run
```
A API estará disponível em `https://localhost:7017` (ou porta configurada).

### 4. Rodar o Frontend (React)
```bash
cd src/Frontend
npm install
npm run dev
```
Acesse `http://localhost:5173` no navegador.

### 5. Docker (opcional)
Há Dockerfiles para backend e frontend. Para rodar ambos via Docker Compose, crie um arquivo `docker-compose.yml` conforme sua necessidade.

## Estrutura de Pastas Importantes
- `src/Frontend/src/pages/` — Páginas principais do app React
- `src/Frontend/src/services/` — Serviços de API (Axios)
- `src/Backend/Paynext.API/Controllers/` — Endpoints da API
- `src/Backend/Paynext.Application/` — Lógica de negócio
- `src/Backend/Paynext.Domain/` — Entidades e regras de domínio
- `src/Backend/Paynext.Infra/` — Persistência de dados
- `src/Backend/Paynext.API.Documents/` — Modelos de respostas

## Funcionalidades
- Autenticação JWT segura
- Gestão de contratos e usuários
- Solicitação, aprovação e rejeição de antecipações
- Interface responsiva e moderna
- Documentação automática da API via Swagger

## Observações
- Removida mudança de senha no update do usuário. Será realizado a mudança em um endpoint proprio no futuro.



## Contato
Dúvidas ou sugestões? Abra uma issue ou envie um e-mail para [fekler.jobs@gmail.com].

---

> Projeto desenvolvido com ❤️ para facilitar a gestão de contratos e antecipações.


