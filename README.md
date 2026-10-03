# 🎮 Vague Echoes: Lost in the Mist

A third-person horror exploration game built with **Unity** and **C#**.

The project combines atmospheric exploration with historical clue discovery, quiz-based progression, difficulty settings, an Echo Sense navigation mechanic, and a monster that actively pressures the player.

---

## 🕹️ Gameplay Overview

The player explores a foggy environment, discovers historical information, completes linked quiz stations, collects progression items, and eventually reaches the exit while avoiding the monster.

Core gameplay loop:

```text
Explore
  ↓
Find and read historical facts
  ↓
Locate the linked quiz station
  ↓
Answer the quiz
  ↓
Collect the clue / key
  ↓
Avoid the monster
  ↓
Reach the exit
```

Wrong answers and Echo Sense usage can increase danger by attracting or activating the monster.

---

## ✨ Implemented Systems

- ✅ Third-person player movement
- ✅ Third-person camera follow
- ✅ Historical fact interaction system
- ✅ Separate quiz stations linked to clues
- ✅ Clue / key collection
- ✅ Objective UI
- ✅ Main menu and intro UI
- ✅ Game over and win states
- ✅ Monster chase behavior
- ✅ Random monster spawn points
- ✅ Wrong-answer monster alert
- ✅ Echo Sense navigation mechanic
- ✅ Difficulty-dependent gameplay behavior
- ✅ Exit gate progression

---

## 🧩 Main Scripts

| Script | Responsibility |
|---|---|
| `GameManager.cs` | Game state, clue progression, monster spawning, win / lose flow |
| `MonsterChaseAI.cs` | Monster detection, chasing, and catching the player |
| `HistoryFact.cs` | Historical fact interaction and reading state |
| `HistoryFactUI.cs` | Historical fact UI presentation |
| `QuizStation.cs` | Controls access to quizzes based on fact progress |
| `BookQuizUI.cs` | Quiz interface and answer handling |
| `ClueItem.cs` | Clue state and progression logic |
| `EchoSense.cs` | Navigation assistance and difficulty-based Echo behavior |
| `DifficultyRuntimeController.cs` | Applies runtime difficulty settings to monster behavior |
| `PlayerInteractor.cs` | Handles player interaction with world objects |
| `SimpleThirdPersonController.cs` | Player movement |
| `ThirdPersonCameraFollow.cs` | Camera follow behavior |
| `GateExit.cs` | Exit / victory interaction |
| `ObjectiveUI.cs` | Objective text display |

---

## 🛠️ Technology

- **Engine:** Unity
- **Language:** C#
- **Gameplay:** MonoBehaviour-based systems
- **Input:** Unity keyboard input
- **Version Control:** Git & GitHub

---

## 🎮 Controls

| Action | Key |
|---|---|
| Move | `W`, `A`, `S`, `D` |
| Interact | `E` |
| Echo Sense | `Q` |
| Restart after game over / win | `R` |

> Controls may depend on the current Unity project configuration.

---

## 📂 Repository Scope

This repository currently focuses on the **C# gameplay scripts** used by the project rather than the complete Unity project files and assets.

Because of that, cloning this repository alone is **not currently enough to run the full game in Unity**.

This repository is intended to showcase the gameplay logic and systems behind *Vague Echoes: Lost in the Mist*.

---

## 📸 Media

Gameplay screenshots and video will be added after the full Unity project is prepared again locally.

---

## 🚧 Future Improvements

- Add gameplay screenshots and a short demo video
- Publish a playable build
- Improve project folder organization
- Add technical documentation for the main gameplay systems
- Continue polishing horror atmosphere, audio, UI, and gameplay feedback

---

## 📚 What I Practiced

This project helped me practice:

- Unity gameplay scripting
- C# component-based architecture
- State and progression management
- Player interaction systems
- UI-driven gameplay
- Basic enemy AI behavior
- Difficulty balancing
- Debugging interconnected game systems

---

## 📄 License

This repository includes an **MIT License**.
