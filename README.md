# MCP-TRPG-Game Server
![GitHub Release](https://img.shields.io/github/v/release/tse-wei-chen/MCP-TRPG-Game) ![GitHub License](https://img.shields.io/github/license/tse-wei-chen/MCP-TRPG-Game) 

An advanced **MCP (Model Context Protocol) powered** TRPG (Tabletop Role-Playing Game) server designed for automated, AI-driven adventures. This project leverages the revolutionary **MCP technology** to seamlessly connect Large Language Models (LLMs) with game mechanics, creating an intelligent AI Keeper (KP) that can dynamically lead, narrate, and manage immersive gaming sessions.

## 🛣️ Roadmap / Future Goals

- [ ] **TRPG MCP Client** — UI for dice, character, multiplayer, and more
- [ ] **Import Your Own Scenarios** — Custom scenario CSV import & editor
- [ ] **Multi-Player Support** — Multiple players, sessions, real-time


## 🎮 Demo Video



https://github.com/user-attachments/assets/237294ee-6db8-4e5e-8d49-f028fc6b50d7



## Key Features

- MCP-powered AI Keeper: Automated, intelligent game master
- Rich MCP toolset: Character, scenario, skill, dice, and event management
- Fast character creation and editing
- Flexible scenario and scene system
- Keeper assistant: NPC dialogue, scene description, random events
- Skill, attribute, sanity checks with dice rolling
- Persistent game records (SQLite)
- CSV seed data for easy import
- Modular service architecture
- Instant game start: Just type "I want to start playing TRPG"
- Language: Traditional Chinese (zh-TW), i18n planned

## Getting Started

### Prerequisites
1. **Install .NET 9.0 SDK**
	 - Download and install [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

2. **MCP-Compatible AI Client** (Optional for enhanced AI integration)
	- Cursor, Claude Desktop, VS Code with MCP extensions, or other MCP-enabled clients
	- This enables seamless AI interaction with the TRPG server through MCP protocol

### Setup & Launch
3. **Build the Project**
	```
	if you are version 1, please delete ./trpg.db file first
	```
	and
	```powershell
	dotnet build
	```

4. **Run the MCP-Enabled TRPG Server**
	<details open>
	<summary><b>Cursor</b></summary>
	
	Go to: `Settings` -> `Cursor Settings` -> `Tools & MCP`
	You should see trpg-mcp in the list and enable it.
	If you don’t see it, click `New MCP Server` and copy and paste the JSON below.
	```json
	{
		"mcpServers": {
			"trpg-mcp": {
				"type": "stdio",
				"command": "dotnet",
				"args": [
					"run" ,
					"--project",
					"src/ToolBox/ToolBox.csproj",
					"--framework",
					"net9.0",
					"--stdio"
				]
			}
		}
	}
	```
	if you want to use http mode
	```bash
	dotnet run --project src/ToolBox/ToolBox.csproj --framework net9.0 -- --stdio
	```
	```json
	{
		"mcpServers": {
			"trpg-mcp": {
				"type": "http",
				"url": "http://localhost:5000/mcp",
				"env": {
					"ASPNETCORE_ENVIRONMENT": "Local"
				}
			}
		}
	}
	```
	
	</details>
	<details open>
	<summary><b>VS Code</b></summary>

	Press `F1` and enter `MCP: List Servers`.  
	You should see **trpg-mcp** in the list—click to start the server.  
	If you don’t see it, press `F1` and enter `MCP: Open User Configuration`, then copy and paste the JSON below.
	```json
	{
		"servers": {
			"trpg-mcp": {
				"type": "stdio",
				"command": "dotnet",
				"args": [
					"run" ,
					"--project",
					"src/ToolBox/ToolBox.csproj",
					"--framework",
					"net9.0",
					"--stdio"
				]
			}
		}
	}
	```

	if you want to use http mode
	```bash
	dotnet run --project src/ToolBox/ToolBox.csproj --framework net9.0
	```
	```json
	{
		"servers": {
			"trpg-mcp": {
				"type": "http",
				"url": "http://localhost:5000/mcp",
				"env": {
					"ASPNETCORE_ENVIRONMENT": "Local"
				}
			}
		}
	}
	```
	</details>

4. **Default Database**
	 - Uses SQLite (`trpg.db`) by default. The database is auto-created on first run.

5. **API Testing**
	 - Use `MCPTRPGGame.http` for sample API requests and testing.

6. **Quick Game Start with MCP**
   - Players can directly send the command "I want to start playing TRPG" via MCP-enabled clients
   - The AI Keeper instantly accesses game tools through MCP to launch dynamic sessions

## 🔧 Available MCP Tools

The server provides 30+ specialized MCP tools for comprehensive TRPG management:

### Core Game Tools
- [x] `StartTrpgAdventure` — Instantly initialize a new TRPG session with step-by-step AI guidance
- [x] `GetAllScenariosAsync` — List all available scenarios for selection
- [x] `GetScenarioByIdAsync` — Get detailed info for a specific scenario
- [x] `GetAllCharactersAsync` — List all player characters
- [x] `GetCharacterByIdAsync` — Get a player character's current status
- [ ] `CreateCharacterAsync` — Create a new player character (CoC7 rules supported)
- [x] `CreateCharacterFromTemplateIdAsync` — 
- [x] `UpdateCharacterAsync` — Update a player character's information
- [x] `UpdateCharacterAttributeAsync` — Update a specific attribute of a player character
- [x] `DeleteCharacterAsync` — Delete a player character by ID
- [x] `GetAvailableCharacterTemplates` — List available character templates for creation

### Dice & Check Tools
- [x] `SkillCheckAsync` — Perform a skill check for a character
- [x] `AttributeCheckAsync` — Perform an attribute check for a character
- [x] `SanityCheck` — Perform a sanity check for a character
- [x] `SavingThrowAsync` — Perform a saving throw for a character
- [x] `CalculateDamageAsync` — Calculate damage for an attack
- [x] `RollDiceAsync` — Roll dice using standard notation (e.g., 1d100)

### AI Keeper Assistant Tools
- [x] `GenerateSceneDescriptionAsync` — Generate a vivid scene description (NPCs, items, actions)
- [x] `GenerateNpcDialogueAsync` — Generate dynamic NPC dialogue suggestions
- [x] `SuggestChecksAndDifficultiesAsync` — Suggest checks and difficulty levels for the current scene
- [x] `GenerateRandomEventAsync` — Generate a random event for the scene or scenario
- [x] `GetGameProgressSuggestionsAsync` — Suggest next steps based on game progress

*All tools are designed for seamless AI integration through the MCP protocol and are accessible to both AI and human Keepers.*

## Directory Structure

src
├── Common (shared models and utilities)
│   └──  seed (initial CSV data)
├── Modules
│   └── Game.Service (module for game logic)
└── ToolBox (tools for seeding and managing data)

## Contact

For questions or suggestions, please contact the project maintainer.
