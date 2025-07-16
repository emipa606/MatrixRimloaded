# GitHub Copilot Instructions for RimWorld Modding Project: MatrixRimloaded

## Mod Overview and Purpose
MatrixRimloaded is a mod for RimWorld that introduces a series of supernatural abilities and interactions inspired by the concept of manipulating the very fabric of reality. This mod aims to enhance gameplay by offering players new strategic options and immersive experiences through custom abilities and visual effects.

## Key Features and Systems
- **Abilities**: Introduces several new abilities such as `Implode`, `JumpOfFaith`, `Phasing`, `SkillHack`, and `WorldTeleportAnomaly`. These abilities offer unique strategic advantages and tactical interactions within the game.
- **Dynamic Objects**: Includes a special `Thing` called `ImplosionExpanding` to enhance in-game effects.
- **Dialogs**: Provides `Dialog_ChooseMatrixSkill` for selecting and managing player abilities.
- **Custom Map Interactions**: Facilitates complex interactions with game maps using abilities such as `WorldTeleportAnomaly`.

## Coding Patterns and Conventions
- **Inheritance**: Many abilities inherit from the `Ability` base class to ensure consistent behavior and interaction mechanisms.
- **Method Structuring**: Methods in abilities are organized by functionality, usually consisting of private helper methods (e.g., `AlliedPawnOnMap`, `ShouldEnterMap`) that encapsulate specific behaviors.
- **Naming Conventions**: Classes and methods follow PascalCase naming for clarity and adherence to C# conventions.

## XML Integration
- **Defining Abilities and Objects**: Utilize XML to define the properties and configurations of new abilities and game objects.
- **Harmonization**: XML definitions are integrated to harmonize new content with existing game elements, ensuring seamless gameplay experiences.
  
## Harmony Patching
- **Patch Management**: The `Projectile_Patch` class is used to apply Harmony patches, modifying existing game code without direct changes for customization and enhancement purposes.
- **Specific Patching**: Utilizes method replacement and prefix/postfix adjustments to alter existing game logic for new abilities and interactions.

## Suggestions for Copilot
- **Generate Helper Methods**: Use Copilot to suggest standard pattern helper methods for interacting with game maps and managing ability effects.
- **XML Enhancements**: Implement suggestions for streamlining XML integration, such as property definitions and patch references.
- **Code Optimization**: Leverage Copilot to recommend optimizations in class interactions, especially where ability effects depend on resource management or map state.
- **Refactoring Proposals**: Seek Copilot to identify potential refactoring opportunities within ability classes, improving maintainability and performance.

This instruction file should guide contributors and developers in understanding the structure and purpose of the MatrixRimloaded mod, offering a framework for utilizing GitHub Copilot effectively during the development process.
