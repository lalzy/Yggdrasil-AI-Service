# Requirements:
- [.net 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-10.0.302-windows-x64-installer)
- EF Core Tools: ```dotnet tool install --global dotnet-ef```
- LLM service (OpenAI-compatible API)


# Initial Setup
1. clone project (see the shiny green button)
2. ```cd {intoFolder}```
3. ```dotnet restore```
4. [create database](#create-the-database)
5. [Run service](#run-the-service).
6. Open browser and go to [http://localhost:5242/](http://localhost:5242/)

### Create the Database
```
dotnet ef migrations add InitialCreate -o src/migrations
dotnet ef database update
```

# Run The Service
Type ```dotnet Run``` in a terminal/command prompt (bash, powershell, cmd) pointed to the root (where you cloned the project to).

# Building (for standalone execution)
```dotnet publish -c Release -r {OS-Architecture} --self-contained -o publish```
Replace {os-architecture} with whatever you have. Example
- Windows: win-x64
- Mac: osx-x64
- Linux: linux-x64
- RaspberryPi: linux-arm64

# Tests
Run tests with ```dotnet test```

# Updating
1. fetch
2. repeat Create the database instructons (change InitialCreate to 'update - ###' or something)

# Project structure

```
src/
  ..Endpoints/ - API endpoint definitions
  ..Models/ - Data Models
  ..Services/ - Service helpers (LLM, Database)
  ..Utils/ - Utility Code.
  ..Pages/ - Razor Pages
  ..Migrations/ - EF Core Migrations.

.Tests/ - Unit tests
```

# API Documentation:
Run the service, then go to [http://localhost:5242/swagger](http://localhost:5242/swagger)
