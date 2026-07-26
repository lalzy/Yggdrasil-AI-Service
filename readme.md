# Yggdrasil
A Scenario/world based web-app for running themed RP sessions with an LLM with multiple-characters. Rather than as Companion or Group-chatting See [Terminology](#terminology) for the distinction.

### Features (To be added):
- Native Narrator vs Character's.
- Native Multi-character in a scenario/world.
- Designated (updated) character-defined slots (such as; Name, Equipment, Personality) that gets sent to the LLM in a way LLMs understand.
- Optional experimental character/narrator separation (utilizing separate LLMs or LLM models per character, and narrator to not have memory bleed)
- Optional experimental way to combat Context-bleed.

### Terminology:
- RP - RolePlay / RolePlaying
- Companion Roleplay - 1:1 conversation with an LLM where the LLM has a 'role'.
- Group Chat - Same as companion Roleplay, but User takes turn between user, and the LLM-Companions inside the groupchat.
- Scenario - the LLM is delegated into being a "narrator" and instead "create" or "use" characters.
- Context - LLMs "short term" memory. Essentially your current chat-log.
- Context bleed - an LLM hallucinate things that occured previously when it shouldn't because it confuse past with present.
- Lorebook - Defines lore/information that will be injected to the chat when certain keywords are triggered (as defined in the lore entries).
- Memory - Longterm memory. Unlike context, it is stored in the Database and added "when relevant".

# Requirements:
- [.net 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-10.0.302-windows-x64-installer)
- EF Core Tools: ```dotnet tool install --global dotnet-ef```
- LLM service (OpenAI-compatible API)


# Initial Setup
1. clone project (see the shiny green button)
2. ```cd {intoFolder}```
3. ```dotnet restore```
4. [Run service](#run-the-service).
5. Open browser and go to [http://localhost:5242/](http://localhost:5242/)

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

# Project structure

```
src/
  ..Controllers/ - API Endpoints
  ..Data/ - Database Definition
  ..DTO/ - Data Transfer Objects
  ..Models/ - Data Models
  ..Services/ - Business Logic
  ..Utils/ - Utility Code.
  ..Pages/ - Razor Pages
Migrations/ - EF Core Migrations. Migrations are auto-ran on dotnet run
.Tests/ - Unit tests
```

# API Documentation:
Run the service, then go to [http://localhost:5242/swagger](http://localhost:5242/swagger)
