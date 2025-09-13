# MCP-TRPG-Game Server

An advanced **MCP (Model Context Protocol) powered** TRPG (Tabletop Role-Playing Game) server designed for automated, AI-driven adventures. This project leverages the revolutionary **MCP technology** to seamlessly connect Large Language Models (LLMs) with game mechanics, creating an intelligent AI Keeper (KP) that can dynamically lead, narrate, and manage immersive gaming sessions.

## 🚀 What is MCP?

**Model Context Protocol (MCP)** is a technology that enables AI models to interact with external tools and data sources in real-time. In MCPTRPGGame, MCP serves as the bridge between the AI and the game world, allowing the AI Keeper to:

- **Access game data dynamically** - Characters, scenarios, NPCs, and game state
- **Execute game mechanics** - Dice rolls, skill checks, combat calculations
- **Modify game world** - Create events, update character stats, progress storylines
- **Provide contextual responses** - Rich, data-driven narrative based on current game state

This makes every gaming session truly adaptive and responsive to player actions!CP-TRPG-Game Server

An advanced TRPG (Tabletop Role-Playing Game) server designed for automated, AI-driven adventures. This project leverages Large Language Models (LLMs) as the Keeper (KP), allowing the AI to lead, narrate, and manage the game, making every session dynamic and immersive.

## 🎮 Demo Video



https://github.com/user-attachments/assets/237294ee-6db8-4e5e-8d49-f028fc6b50d7



## Key Features

- **🔗 MCP-Powered AI Keeper (KP):**
	- Leverages **Model Context Protocol** for seamless AI-game integration
	- The AI Keeper dynamically accesses and manipulates game data through MCP tools
	- Real-time contextual decision making based on current game state
	- Supports fully automated game sessions with intelligent event generation and story progression

- **🛠️ Rich MCP Tool Integration:**
	- 30+ specialized MCP tools for comprehensive game management
	- Character creation, skill checks, combat assistance, and scenario management
	- Dynamic NPC dialogue generation and scene description enhancement
	- Automated game progress tracking and decision suggestions

- **Character Creation & Management:**
	- Create, edit, and manage player and non-player characters.
	- Supports custom attributes, skills, sanity (SAN) checks, and inventory.

- **Scenario & Scene Management:**
	- Flexible scenario system for custom adventures.
	- Scene descriptions, hidden elements, and dynamic event generation.

- **Keeper Assistant Tools:**
	- Random event generator, NPC dialogue engine, scene description enhancer.
	- Automated suggestions for game progress and dice rolls.

- **Skill, Attribute, and Sanity Checks:**
	- Automated dice rolling for skills, attributes, and sanity.
	- Customizable difficulty and result interpretation.

- **Game Progress Tracking:**
	- Persistent game records, timeline management, and event logging.
	- SQLite database for reliable data storage.

- **🌐 MCP-Enabled API Architecture:**
	- RESTful API enhanced with MCP protocol for AI integration
	- Seamless connection between AI models and game mechanics
	- Easily integrate with MCP-compatible AI clients, frontends, bots, or other tools
	- Real-time bidirectional communication between AI and game server

- **Seed Data Loader:**
	- CSV-based initial data for skills, scenarios, NPCs, scenes, and more.

- **Extensible Service Architecture:**
	- Modular services for character, scenario, random elements, and more.

- **Multi-Player Support:**
	- Manage multiple players, sessions, and game states concurrently.

- **Customizable Rules:**
	- Built-in support for CoC7 rules, but easily extendable for other TRPG systems.

- **Automated Testing & API Examples:**
	- Example HTTP requests in `MCPTRPGGame.http` for quick API testing.

- **Instant Game Start for Players:**
  - Players can simply input "I want to start playing TRPG" to immediately begin a new TRPG adventure, with the LLM Keeper guiding the session from the start.

- **Language Support:**
	- Currently, the main supported language is Traditional Chinese (zh-TW).
	- Future releases aim to support i18n (internationalization) for multi-language gameplay and UI.

## Getting Started

### Prerequisites
1. **Install .NET 9.0 SDK**
	 - Download and install [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

2. **MCP-Compatible AI Client** (Optional for enhanced AI integration)
	 - Claude Desktop, VS Code with MCP extensions, or other MCP-enabled clients
	 - This enables seamless AI interaction with the TRPG server through MCP protocol

### Setup & Launch
3. **Build the Project**
	 ```powershell
	 dotnet build
	 ```

4. **Run the MCP-Enabled TRPG Server**
	 ```powershell
	 dotnet run
	 ```
	 The server will start with MCP tools available for AI integration

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
- `start_trpg_adventure` - Instant game initialization
- `create_character` - Dynamic character creation
- `roll_skill_check`, `roll_attribute_check`, `roll_sanity_check` - Dice mechanics
- `combat_assistance` - Battle management

### AI Keeper Assistant Tools  
- `generate_npc_dialogue` - Dynamic NPC interactions
- `generate_scene_description` - Rich environmental storytelling
- `generate_random_event` - Adaptive story progression
- `get_game_progress_suggestion` - Intelligent session guidance

### Session Management
- `create_game_session`, `log_game_event` - Session tracking
- `get_scenario_info`, `get_scene_info` - World state access
- `update_character_hit_points` - Real-time character management

*All tools are designed for seamless AI integration through the MCP protocol.*

## Directory Structure

- `Controllers/` — Tool controllers
- `Models/` — Data models
- `Services/` — Business logic and game services
- `Data/` — Database context
- `seed/` — Initial CSV data files

## Contact

For questions or suggestions, please contact the project maintainer.
