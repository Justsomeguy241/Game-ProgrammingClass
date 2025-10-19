## Developer & Contributions

Muhammad Rafi Ramadhan (Game Developer and Game Designer)
  <br>

## About

Space Invaders Evolved is a fast-paced arcade shooter where you face endless waves of alien enemies. Multiple enemy types keep you on your toes bombers that dive in for explosive damage, and snipers that fire deadly lasers to block off sections of the arena. Power-ups drop from enemies giving temporary boosts like explosive bombs, knockback blasts, and multishot upgrades that can stack up to four shots. The longer you survive, the harder and more chaotic the battle becomes.
<br>

## Key Features & Personal Contributions
Core Gameplay Features
- **Endless Wave System** — Implemented continuous enemy spawning with progressive difficulty scaling.
- **Universal Enemy Behavior System** — Designed a shared behavioral framework governing all enemy types:
  - Enemies descend through six vertical tiers, each representing a phase of approach toward the player.
  - Tier transitions are triggered by the number of wall bounces, with each enemy type having distinct descent speeds and bounce limits.
  - Upon reaching the final (sixth) tier, all enemies switch to aggressive pursuit mode, directly chasing the player and detonating on impact as a punishment mechanic for delayed elimination.
- **Enemy Types & Behaviors**  
  - **Strikers:** Standard enemies that swarm the player in large numbers, firing straightforward projectiles and using group-based movement patterns.
  - **Bombers:**  Slow-moving enemies that drop bombs creating area-of-effect explosions; maintain steady movement and drop rates regardless of wave speed.
  - **Snipers:** Faster enemies that descend through tiers quicker than others, firing lasers that block off sections of the arena.
- **Power-Up System** — Developed instant-activation power-ups with timed effects and stackable upgrades, including:
  - **Explosive Bullets:** Projectiles that detonate with splash damage.
  - **Knockback Bullets:**  Temporary stun and pushback effect on enemies hit.
  - **Multishot Upgrade:** Expands firing capacity up to four simultaneous projectiles.
- **Combat & Player Control** — Designed responsive shooting mechanics, movement controls, and hit detection for a fluid arcade experience.
- **UI and Game Flow** — Created scoring, wave tracking, and game-over logic with corresponding user interface elements.
  
<h3>Personal Contributions</h3>
<p><em>Solo Developer – responsible for full development cycle</em></p>

| **System / Feature**                | **Description**                                                                                                                                                | **Time Spent** |
| ----------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------- | -------------- |
| **Gameplay Architecture**           | Designed and implemented the main game loop, wave progression, and difficulty scaling systems.                                                                 | ~6 hours       |
| **Universal Enemy Behavior System** | Built a shared behavioral framework controlling tier-based descent, bounce tracking, and final pursuit mechanics across all enemy types.                       | ~6 hours       |
| **Enemy AI & Behaviors**            | Programmed three enemy archetypes (Strikers, Bomber, Sniper) with individual movement, attack, and descent-speed parameters integrated into the universal system. | ~5 hours       |
| **Power-Up Mechanics**              | Implemented a system for instant-activation power-ups with time-based effects and stackable upgrades.                                                          | ~4 hours       |
| **Player Controls & Combat**        | Developed responsive player control, shooting, and collision systems for consistent arcade gameplay.                                                           | ~5 hours       |
| **UI / Game States**                | Implemented UI components for score tracking, wave indicators, and game-over transitions.                                                                      | ~3 hours       |
| **Balancing & Playtesting**         | Tuned enemy speed, bounce count thresholds, and tier timings for balanced pacing and fair difficulty scaling.                                                  | ~4 hours       |
| **Audio & Visual Integration**      | Integrated explosion effects, laser visuals, and impact sounds to reinforce gameplay feedback.                                                                 | ~4 hours       |

Total Development Time: ~37 hours

<div align="center">
  <table width="30%" style="border-collapse: collapse; border: 2px solid #ccc;">
    <tr>
      <td align="center" style="padding: 0;">
        <img src="https://github.com/Justsomeguy241/Justsomeguy241/blob/main/Space%20Invaders.gif" alt="gameplay demo" width="100%">
      </td>
      <td align="center" style="padding: 0;">
        <img src="" alt="gameplay demo2" width="100%">
      </td>
    </tr>
  </table>
</div>




<h2>Scene Flow</h2>

<div align="center" style="transform: scale(0.85); transform-origin: top center;">

```mermaid
flowchart LR
  mm[Main Menu]
  gp[Gameplay]
  es[End Screen]

  mm -- "Start Game" --> gp
  gp -- "Player Death" --> es
  es -- "Play Again" --> gp
```
</div> 

## Layer / Module Design 

```mermaid
---
config:
  theme: neutral
  look: neo
---
graph TD
    %% ====== INITIALIZATION ======
    subgraph "Initialization Phase"
        Start([Game Start])
        BootNote([Boot Sequence:<br/>Load Managers, Systems, and First Scene])
    end

    %% ====== CORE SYSTEMS ======
    subgraph "Core Systems"
        InputManager[Input Manager]
        AudioManager[Audio Manager]
        SceneLoader[Scene Loader]
        UIManager[UI Manager]
    end

    %% ====== GAMEPLAY ======
    subgraph "Gameplay Logic"
        GameManager[Game Manager]
        PlayerController[Player Controller]
        SwitchSystem[Character Switch System]
        Fox[Fox Controller]
        Crow[Crow Controller]
        PuzzleSystem[Puzzle / Physics System]
    end

    %% ====== USER INTERFACE ======
    subgraph "UI System"
        MainMenu[Main Menu]
        PauseMenu[Pause Menu]
        HUD[In-Game HUD]
        VictoryScreen[Victory / End Screen]
        Settings[Settings Menu]
    end

    %% ====== FLOW CONNECTIONS ======
    Start --> BootNote
    BootNote --> InputManager
    BootNote --> AudioManager
    BootNote --> SceneLoader
    BootNote --> UIManager

    %% Gameplay Links
    InputManager --> PlayerController
    PlayerController --> SwitchSystem
    SwitchSystem --> Fox
    SwitchSystem --> Crow
    PlayerController --> PuzzleSystem
    PuzzleSystem --> GameManager
    GameManager --> UIManager

    %% UI Links
    UIManager --> MainMenu
    UIManager --> PauseMenu
    UIManager --> HUD
    UIManager --> VictoryScreen
    UIManager --> Settings
    MainMenu --> GameManager
    PauseMenu --> GameManager
    HUD --> PlayerController
    VictoryScreen --> MainMenu

    %% Styling
    classDef initStyle fill:#e1f5fe,stroke:#01579b,stroke-width:2px
    classDef systemStyle fill:#ede7f6,stroke:#4a148c,stroke-width:2px
    classDef gameplayStyle fill:#e8f5e9,stroke:#1b5e20,stroke-width:2px
    classDef uiStyle fill:#fff3e0,stroke:#e65100,stroke-width:2px

    class Start,BootNote initStyle
    class InputManager,AudioManager,SceneLoader,UIManager systemStyle
    class GameManager,PlayerController,SwitchSystem,Fox,Crow,PuzzleSystem gameplayStyle
    class MainMenu,PauseMenu,HUD,VictoryScreen,Settings uiStyle

```


## Modules and Features

The advanced 2D platformer systems — including character switching, puzzle interactions, level progression, and dynamic audio — are powered by a modular scripting structure.
Each module is responsible for managing specific aspects of gameplay, UI, or global systems to ensure smooth transitions and responsive player control.

| 📂 **Name**                 | 🎬 **Scene / Scope** | 📋 **Responsibility**                                                                                                                                    |
| --------------------------- | -------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **GameManager**             | **Gameplay**         | - Controls overall game state (playing, paused, level complete, failed)<br/>- Coordinates transitions between menus, levels, and victory screens         |
| **AudioManager**            | **Global**           | - Manages background music (OST) and sound effects (SFX)<br/>- Handles audio settings such as volume and mute controls                                   |
| **MainMenu**                | **Main Menu**        | - Displays the main menu interface<br/>- Provides access to level selection, settings, and exit options                                                  |
| **OptionsPanel**            | **Main Menu**        | - Adjusts and applies user preferences (e.g., audio volume)<br/>- Saves settings for future sessions                                                     |
| **LevelsPanel**             | **Main Menu**        | - Handles level selection<br/>- Loads the selected scene or level into gameplay                                                                          |
| **Canvas / UI (Gameplay)**  | **Gameplay**         | - Manages on-screen UI elements such as HUD, pause menu, and level complete screens<br/>- Displays player feedback and progress indicators               |
| **Rotator (Flip System)**   | **Gameplay**         | - Handles level rotation mechanics<br/>- Dynamically alters the environment to reveal new paths or puzzle solutions                                      |
| **Puzzle Objects (Grid)**   | **Gameplay**         | - Manages interactable puzzle elements (e.g., levers, doors, buttons, pop-up walls)<br/>- Responds to player interactions and triggers logical responses |
| **LargeMoveableBox**        | **Gameplay**         | - Heavy interactable object<br/>- Can be pushed or pulled by the **Fox** but not lifted<br/>- Used in environmental puzzles                              |
| **SmallTrash**              | **Gameplay**         | - Lightweight object<br/>- Can be picked up and carried by the **Crow** to solve certain puzzles                                                         |
| **Fox**                     | **Gameplay**         | - Playable character focused on strength-based actions<br/>- Can wall-jump, push, and pull heavy objects                                                 |
| **Crow**                    | **Gameplay**         | - Playable character focused on agility<br/>- Can fly and carry light objects to assist in puzzle-solving                                                |
| **Character Switch System** | **Gameplay**         | - Handles input-based switching between the Fox and the Crow<br/>- Ensures smooth transitions while retaining control context                            |
| **FoxAndCrowDetector**      | **Gameplay**         | - Detects when both Fox and Crow reach the exit area<br/>- Triggers level completion event through **GameManager**                                       |
| **ExitDoorR**               | **Gameplay**         | - Represents the exit point of each level<br/>- Works with **FoxAndCrowDetector** to activate victory conditions                                         |





<br>


## Game Flow Chart


```mermaid
---
config:
  theme: redux
  look: neo
---
flowchart TD
  %% --- Game Start & Menu ---
  start([Game Start])
  start --> menu{Main Menu}

  menu -->|"Select Level"| level[Load Chosen Level]
  menu -->|"Settings"| settings[Adjust Audio/Preferences]
  menu -->|"Quit"| quit([Exit Game])
  settings --> menu

  %% --- Gameplay Core ---
  level --> input{Player Input}
  input -->|"Move / Jump"| move[Apply Movement & Physics]
  input -->|"Switch Character"| switch[Switch Between Fox & Crow]

  %% --- Character Abilities ---
  switch --> fox[Fox: Wall Jump, Push/Pull Box]
  switch --> crow[Crow: Fly, Carry Light Box]

  %% --- Puzzle System ---
  move --> puzzle{Puzzle Interaction}
  fox --> puzzle
  crow --> puzzle

  puzzle --> lever[Levers / Buttons / Doors]
  puzzle --> box[Moveable Boxes]
  puzzle --> wall[Popup Walls / Obstacles]

  %% --- Flip Mechanic ---
  move --> flip{Flip Triggered?}
  flip -->|Yes| doFlip[Rotate/Flip Level Layout]
  flip -->|No| cont[Continue Gameplay]

  %% --- Exit System ---
  cont --> exitChk{Both Fox & Crow at Exit?}
  doFlip --> exitChk

  exitChk -->|Yes| complete[Level Complete Panel]
  exitChk -->|No| input

  complete --> backToSelect[Return to Level Select]
  backToSelect --> menu


```


<br>

## Event Signal Diagram


```mermaid
%%{init: {'theme':'neutral', 'scale': 0.1}}%%
classDiagram
    class PlayerController {
        +Move(direction: Vector2)
        +Jump()
        +SwitchCharacter()
    }

    class Fox {
        +WallJump()
        +PushBox()
        +PullBox()
    }

    class Crow {
        +Fly()
        +CarryBox(weight: float)
    }

    class PuzzleSystem {
        +ActivateLever()
        +PressButton()
        +OpenDoor()
    }

    class Box {
        +Move()
        +IsCarryable(): bool
    }

    class FlipSystem {
        +TriggerFlip()
        +RotateLevel()
    }

    class GameManager {
        +StartLevel(levelName: string)
        +CompleteLevel()
    }

    class UIManager {
        +ShowPauseMenu()
        +ShowVictoryScreen()
    }

    class AudioManager {
        +OnPlayBGM(trackName: string)
        +OnPlaySFX(effectName: string)
    }

    PlayerController --> Fox : controls
    PlayerController --> Crow : controls
    PlayerController --> PuzzleSystem : interacts
    Fox --> Box : pushes/pulls
    Crow --> Box : carries
    PuzzleSystem --> Box : requires
    FlipSystem --> PuzzleSystem : flips
    FlipSystem --> Fox : flips
    FlipSystem --> Crow : flips
    GameManager --> UIManager : manages
    GameManager --> AudioManager : triggers
    GameManager --> FlipSystem : activates

```


<br>
