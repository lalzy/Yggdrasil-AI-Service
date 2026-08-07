# Yggdrasil

A scenario based web-app for running RP Sessions with an LLM, (contra companion or group-chatting, see [Terminology](#terminology) for the distinction).

**World:**
The world is where the RP takes place (can be based on reality, on fiction, whatever you think of). A world can contain 0 or more characters, and 0 or more lorebooks.

**Character:**
A "protagonist" of sort, that the LLM controls. They can be antagonists, companions, or simply background characters of some import or note. Characters can be shared with any number of worlds and is not tied to a specific world.

**Persona:**
The user "character". How you want the characters/narrator to 'see' or 'understand' what 'you' are.

### Current State:
Working towards MVP (Progress can be seen on the public [Trello](https://trello.com/b/1AmBLJMv)) but in essence, the MVP will have these features:
- Ability to create worlds, characters and Personas.
- Send messages To Llama.Cpp and OpenRouter API.
- Store Worlds, chats (within the world), and characters for persistent use.

### Plan / Future Features:
- Native Narrator + Multiple character support for an RP Scenario (No Lorebook juggling).
- Custom location-fields in Lorebook (with summary versus detailed entry for districts etc).
- Narrative in-world location tracking (space, time, weather). Paired with location fields above to get the relevant location data at all times.
- Character stat tracking (location, relationship/disposition, etc).
- (optional and experimental) Character / Narrator separation using different LLM calls for Narrator versus Character.
- (optional) Memory storage per character(per world), to be used with the separation of LLM Calls.
- (optional) Experimental attempt to combat context-bleed (will detail when implemented).
- SillyTavern character card import support.
- Export world/character support
- plugins

### Terminology:
- RP - RolePlay / RolePlaying
- Companion Roleplay - 1:1 conversation with an LLM where the LLM has a 'role'.
- Group Chat - Same as companion Roleplay, but User takes turn between user, and the LLM-Companions inside the groupchat.
- Scenario - the LLM is delegated into being a "narrator" and instead "create" or "use" characters.
- Context - LLM's "short term" memory. Essentially your current chat-log.
- Context bleed - an LLM hallucinate things that occured previously when it shouldn't because it confuse past with present.
- Lorebook - Defines lore/information that will be injected to the chat when certain keywords are triggered (as defined in the lore entries).
- Memory - Longterm memory. Unlike context, it is stored in the Database and added "when relevant".

# Requirements:
- [.net 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
- EF Core Tools: ```dotnet tool install --global dotnet-ef```
- OpenAI-compatible LLM-Keys (either through something like openrouter, or running your own with llama.cpp)


# Initial Setup
1. clone project (see the shiny green button)
2. ```cd {intoFolder}```
3. ```dotnet restore```
4. [Run service](#run-the-service) (or [run tests](#tests))
5. Open browser and go to [http://localhost:5242/](http://localhost:5242/)

# Run The Service
Type ```dotnet run``` in a terminal/command prompt (bash, powershell, cmd) pointed to the root (where you cloned the project to).

# Building (for standalone execution)
```dotnet publish -c Release -r {OS-Architecture} --self-contained -o publish```
Replace {OS-architecture} with whatever you have. Example
- Windows: win-x64
- Mac: osx-x64
- Linux: linux-x64
- RaspberryPi: linux-arm64

# Tests
Run tests with ```dotnet test```

# Project structure

```
src/
  Constants/ - Static Objects holding Static values.
  Controllers/ - API Endpoints
  Data/ - Database Definition
  DTO/ - Data Transfer Objects
  Models/ - Data Models
  Pages/ - Razor Pages
  Services/ - Business Logic
  Utils/ - Utilities.
Migrations/ - EF Core Migrations. Migrations are auto-ran on dotnet run
.Tests/ - Unit tests
```

# API Documentation:
Run the service, then go to [http://localhost:5242/swagger](http://localhost:5242/swagger)
