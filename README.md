# Animal Rescue Dashboard

A full-stack animal rescue dashboard built with Blazor WebAssembly,
a .NET API, Entity Framework Core, and PostgreSQL.

The dashboard supports searching, filtering, sorting, and pagination
of animal records, along with breed statistics and an interactive map.

This project was developed for my SNHU computer science capstone,
enhancing an earlier Python Dash and MongoDB application.

[Live Demo](https://ptaylor-dev.github.io/RescueAnimalDashboard/)

## Prerequisites

- .NET 10 SDK
- Git
- Docker Desktop, or Docker Engine with Docker Compose
- An available local port `5432` for PostgreSQL

The instructions below run PostgreSQL in Docker and run the API and
frontend locally with the .NET CLI.

## 1. Clone and Build

```bash
git clone [https://github.com/ptaylor-dev/RescueAnimalDashboard.git](https://github.com/ptaylor-dev/RescueAnimalDashboard.git)
cd RescueAnimalDashboard

dotnet restore RescueAnimalDashboard.slnx
dotnet build RescueAnimalDashboard.slnx
```

All remaining commands assume the repository root is the current directory.

## 2. Configure Local HTTPS

Create and trust the ASP.NET Core development certificate:

```bash
dotnet dev-certs https --trust
```

Approve the certificate trust prompt if one appears.

## 3. Start PostgreSQL

Start Docker, then run:

```bash
docker compose up -d db
```

Check the database status:

```bash
docker compose ps
```

Wait until the database reports a healthy status before starting the API.

The Compose configuration uses PostgreSQL 18 with the following
development settings:

| Setting | Value |
|---|---|
| Host | `localhost` |
| Port | `5432` |
| Database | `rescueanimals` |
| Username | `rescue` |
| Password | `rescue_dev_password` |
| Container | `rescue-db` |

The database uses a named volume to preserve data between container restarts.

## 4. Check API Configuration

In `RescueAnimalDashboard.API/appsettings.Development.json`,
ensure the following settings are present. Preserve any other
settings in the file.

```json
{
  "ConnectionStrings": {
    "RescueAnimals": "Host=localhost;Port=5432;Database=rescueanimals;Username=rescue;Password=rescue_dev_password"
  },
  "Cors": {
    "AllowedOrigins": [
      "[https://localhost:7111](https://localhost:7111)"
    ]
  }
}
```

These credentials are intended for local development only.
Do not reuse them in production or commit production credentials.

## 5. Start the API

```bash
dotnet run --project RescueAnimalDashboard.API/RescueAnimalDashboard.API.csproj
