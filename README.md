# Endless Runner Shooter

Welcome to the **Endless Runner Shooter** project! This repository contains a Unity-based 3D endless runner game with integrated shooting mechanics. This was my second game project, showcasing visually appealing gameplay, intuitive controls, and polished features.

## Table of Contents
- [Features](#features)
- [Installation](#installation)
- [Gameplay](#gameplay)
- [Bonuses](#bonuses)
- [Tiles](#tiles)
- [Assets and Visuals](#assets-and-visuals)
- [Known Issues](#known-issues)
- [Future Development](#future-development)
- [License](#license)

---

## Features
- **Endless Running**: Navigate through an infinite, procedurally generated world.
- **Shooting Mechanic**: Engage obstacles by clicking the left mouse button. 
- **Three Bonuses**: Collect bonuses to enhance your performance during gameplay.
- **Dynamic Tiles**: Currently includes three unique tiles, with six more under development.
- **High-Quality Animations**: Integrated animations from Mixamo for smooth and realistic character movements.
- **Visual Effects (VFX)**: Enhanced VFX for an immersive experience.
- **Polished UI**: A clean, responsive, and user-friendly interface tailored for the game.
- **Dynamic Difficulty Scaling**: Game speed increases as the player progresses, intensifying the challenge.
- **Mobile Accessibility**: Basic mobile compatibility with planned support for sliding gestures.

## Installation
1. **Clone the repository**:
   ```bash
   git clone https://github.com/batuhancetinkaya1/Endless-Runner--Shooter-.git
   ```
2. **Open the project** in Unity (tested with Unity version 2022.3.44f1).
3. **Load the main scene** and press the Play button in the Unity Editor to start the game.

## Gameplay
Your goal is to run endlessly while avoiding obstacles and collecting bonuses to maximize your score. Progressively challenging gameplay ensures an engaging experience.

### Controls
- **Desktop**: Use arrow keys or `A/D` to move, and click the left mouse button to shoot.
- **Mobile**: Tap to shoot; slide gestures for movement will be added soon.

## Bonuses
1. **Shield**: Grants temporary immunity to obstacles, allowing safe passage.
2. **Magnet**: Attracts nearby collectible items, making them easier to obtain.
3. **Fire Rate Boost**: Temporarily increases the player's shooting speed, enabling faster obstacle clearing.

### Bonus Mechanics
- Bonuses appear periodically along the runner path.
- The **Fire Rate Boost** is spawned using a weighted random mechanic based on player progression and current performance.

## Tiles
- **Current Tiles**: Three unique tiles that alternate to create variety.
- **Under Development**: Six additional tiles are being designed to enhance gameplay diversity.

## Dynamic Difficulty Scaling
- As the player progresses, the game speed increases gradually.
- Speed scaling is determined by:
  ```
  CurrentSpeed = BaseSpeed + (TimeElapsed * SpeedMultiplier)
  ```
  - `BaseSpeed`: The starting speed of the game.
  - `TimeElapsed`: The total time the player has survived.
  - `SpeedMultiplier`: A factor that determines the rate of speed increase.

## Assets and Visuals
- **Animations**: Mixamo animations for smooth transitions and realistic character movements.
- **Visual Effects**: Custom VFX for shooting, obstacle destruction, and bonus activation.
- **Environment**: Includes professionally designed assets for obstacles and backgrounds.
- **User Interface**: Designed to complement the game's aesthetic and provide clear feedback.

## Known Issues
- **FPS Drops**: Performance degradation during extended gameplay. Optimizations are planned to address this.
- **Mobile Gestures**: Sliding mechanics for movement are not implemented yet.

## Future Development
- **Tile Expansion**: Complete the six additional tiles under construction.
- **Obstacle Variety**: Introduce new types of obstacles with unique behaviors.
- **Mobile Controls**: Implement and refine sliding gestures for a better mobile experience.
- **Enhanced Performance**: Optimize performance to eliminate FPS drops during gameplay.
- **Leaderboard**: Add a global leaderboard to encourage competition among players.

## License
This project is open source and available under the [MIT License](LICENSE). Feel free to use, modify, and distribute the code as per the license terms.

---

Play the game on [itch.io](https://batuhancetinkaya.itch.io/endless-runner) and share your feedback!
