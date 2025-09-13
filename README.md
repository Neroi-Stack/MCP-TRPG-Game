# MCPTRPGGame Server

An advanced TRPG (Tabletop Role-Playing Game) server designed for automated, AI-driven adventures. This project leverages Large Language Models (LLMs) as the Keeper (KP), allowing the AI to lead, narrate, and manage the game, making every session dynamic and immersive.

## 🎮 Demo Video

<video src=".readme/TRPG-demo.mp4" controls="controls" width="500" height="300"></video>

## Key Features

- **LLM as Keeper (KP):**
	- The AI acts as the game master, narrating scenes, generating NPC dialogues, and driving the story forward.
	- Supports fully automated game sessions, with the LLM making decisions, creating events, and responding to player actions.

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

- **API-Driven Gameplay:**
	- RESTful API for all game operations.
	- Easily integrate with frontends, bots, or other tools.

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

1. **Install .NET 9.0 SDK**
	 - Download and install [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

2. **Build the Project**
	 ```powershell
	 dotnet build
	 ```

3. **Run the Server**
	 ```powershell
	 dotnet run
	 ```

4. **Default Database**
	 - Uses SQLite (`trpg.db`) by default. The database is auto-created on first run.

5. **API Testing**
	 - Use `MCPTRPGGame.http` for sample API requests and testing.

6. **Quick Game Start**
   - Players can directly send the command "I want to start playing TRPG" via the API or supported interface to instantly launch a new game session led by the AI Keeper.

## Directory Structure

- `Controllers/` — Tool controllers
- `Models/` — Data models
- `Services/` — Business logic and game services
- `Data/` — Database context
- `seed/` — Initial CSV data files

## Contact

For questions or suggestions, please contact the project maintainer.
