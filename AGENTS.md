# AGENTS.md - AI Agent Guide for Mothership

This document provides context for AI agents working on this codebase.

## Project Overview

**Mothership** is a Unity-based node-based spaceship builder and flight simulator. Players construct modular spaceships by connecting nodes (thrusters, panels, structural pieces) and then fly their creations in 3D space with realistic physics.

## Tech Stack

- **Engine**: Unity 2021.3.10 LTS
- **Language**: C#
- **Input System**: Unity New Input System
- **Physics**: 3D Rigidbody with custom thrust simulation

## Architecture Overview

```
Assets/
├── Code/                    # Core game logic
│   ├── Ship/               # Ship and construction systems
│   │   ├── Ship.cs         # Main ship component, inherits Construct
│   │   ├── ShipController.cs    # Flight control and PID rotation
│   │   ├── BuildShip.cs
│   │   └── Construct.cs    # Base class for constructible objects
│   ├── Nodes/              # Node system
│   │   ├── Node.cs         # Base node class (~1,272 lines)
│   │   ├── Truster.cs      # Thruster/engine nodes
│   │   ├── Panel.cs        # Structural panel nodes
│   │   ├── NodeController.cs    # Node registry and lookup
│   │   └── Connection*.cs  # Connection point system
│   ├── PIDcontroller.cs    # PID controller for rotation
│   ├── AsteroidSpawner.cs  # Procedural asteroid generation
│   ├── OrbitCamera.cs      # Camera control system
│   └── Utility.cs          # Helper functions
├── Player/                  # Player input and UI
│   ├── PlayerMovementController.cs
│   ├── PlayerUI.cs
│   ├── BuildMode.cs
│   ├── NodePlacer.cs
│   ├── PlayerControls.cs   # Input bindings
│   └── PlayerCommands/     # Command pattern implementation
├── Editor/                  # Unity Editor tools
├── Scenes/                  # Game scenes
├── Materials/
└── Resources/              # Prefabs and meshes
    └── Meshes/             # Node shape FBX models
```

## Key Systems

### 1. Node System (`Assets/Code/Nodes/`)

The foundation of the game. Nodes are modular components that connect together.

- **Node.cs**: Base class for all placeable nodes
  - Port management (connection points)
  - Connection validation via overlap sphere raycasts
  - Bridge detection algorithm (flood-fill graph traversal)
  - Breadth-first search for connectivity testing

- **ConnectionPoint**: Typed ports for connecting nodes
  - Type ID determines compatibility
  - Transform-based positioning
  - Connection state tracking

- **Node Types**:
  - `Truster.cs`: Propulsion engines with force vectors
  - `Panel.cs`: Structural components

### 2. Ship System (`Assets/Code/Ship/`)

- **Construct.cs**: Base class for any constructible object
  - Node management (add/remove)
  - Break-off handling when nodes disconnect

- **Ship.cs**: Inherits from Construct
  - Rigidbody physics integration
  - Dynamic mass and center of mass calculation
  - Truster array management
  - Bridge detection for breaking apart ships

- **ShipController.cs**: Flight control
  - Thruster input processing
  - PID controller for rotation to camera direction
  - Dampening system (momentum cancellation)

### 3. Player System (`Assets/Player/`)

- **BuildMode.cs**: Toggle between build and flight modes
- **NodePlacer.cs**: Node placement with cursor preview
- **PlacementCast.cs**: Raycast-based placement detection
- **PlayerUI.cs**: HUD (speed, dampening status, node info)
- **PlayerMovementController.cs**: Direct player capsule movement

### 4. Camera System

- **OrbitCamera.cs**: Free orbit camera around target
  - Mouse/keyboard control
  - Distance clamping
  - Geometry occlusion handling

### 5. Physics

- Mass calculated as sum of all node masses
- Custom inertia tensor calculation
- Trusters apply forces at their world positions
- PID controller for smooth rotation

### 6. Input System

Uses Unity's New Input System with action maps:
- `Movement`: WASD, mouse look, analog sticks
- `Build`: Placement, rotation, removal actions

## Key Algorithms

### Bridge Detection
When a node is removed, the system detects if the ship should break apart:
1. Flood-fill from the "core" node
2. Any unreachable nodes form a new construct
3. Uses breadth-first search for connectivity

### Thruster Force Application
Trusters apply forces based on:
- Their position relative to center of mass
- Their facing direction
- Input magnitude from player

### PID Controller
Used for smooth rotation:
- Proportional: Current error
- Integral: Accumulated error over time
- Derivative: Rate of change of error

## Important Notes for Agents

1. **Prefabs are binary**: Unity prefabs can't be read as text. Rely on C# scripts for logic understanding.

2. **Node.cs is complex**: At ~1,272 lines, it handles connection logic, validation, and graph algorithms.

3. **Physics-based gameplay**: Changes to mass, force application, or inertia calculations significantly affect gameplay feel.

4. **Command Pattern**: Build commands use the command pattern for potential undo/redo support.

5. **Port system is type-based**: Connection compatibility is determined by port type IDs, not geometry.

## Common Tasks

### Adding a New Node Type
1. Create new class inheriting from `Node`
2. Define connection points with appropriate types
3. Add to `NodeController` registry
4. Create prefab with mesh and collider

### Modifying Flight Controls
- Edit `ShipController.cs` for thrust/rotation behavior
- Adjust PID values in `PIDcontroller.cs` for rotation feel
- Input bindings in `PlayerControls.inputactions`

### Changing Build Mode Behavior
- `BuildMode.cs` manages mode state
- `NodePlacer.cs` handles placement logic
- `PlacementCast.cs` for raycast targeting

## Codebase Statistics

- ~36 C# scripts
- ~2,900 lines of code
- 5 mesh assets (FBX)
- Multiple test scenes
