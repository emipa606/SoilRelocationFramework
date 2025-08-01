# GitHub Copilot Instructions for Soil Relocation Framework (Continued)

## Mod Overview and Purpose

**Mod Name:** Soil Relocation Framework (Continued)

This mod is an updated continuation of Evelyn's original project. Its primary function is to introduce a "Dig" order in RimWorld allowing players to extract and relocate soil and various terrains. By giving players the ability to move and manipulate terrains, the mod adds strategic depth to resource management, making it possible to transform environments for different farming, construction, and strategic goals.

## Key Features and Systems

- **Terrain Dig and Place System:** Implement a "Dig" order that provides resource items when executed. These items can then be placed down in the "Floors" menu, functioning identically to the original terrain.
- **Fertile Soil Management:** Reorganize or relocate patches of fertile soil to optimize farming areas.
- **Custom Terrain Management:** Remove soil from unwanted areas, fill water cells to create usable land, and remove wet terrain.
- **Temperature Control with Natural Resources:** Extract ice to maintain cooler room temperatures, useful for maintaining medieval or tribal freezers.
- **Fire Prevention:** Alter terrain to reduce fire spread risks.
- **Optional Recipes and Patches:** Grind stone to sand with recipes and apply optional patches to adjust base game and other mods' balances.
- **Compatibility with Other Terrain Mods:** Works seamlessly with mods that introduce new soil types or modify existing terrain behaviors.
- **Multiplayer-Friendly:** Tested and works properly in multiplayer settings.

## Coding Patterns and Conventions

When contributing to or maintaining this mod, follow these conventions:

- Use **PascalCase** for class and method names.
- Use **camelCase** for local variables and method parameters.
- Organize code into partial classes where applicable to decompose large classes logically.
- Include XML documentation comments for public classes and methods.
- Keep method responsibilities focused and classes small, adhering to the Single Responsibility Principle.

## XML Integration

The mod relies on XML for defining and handling terrain types. Copilot should consider XML handling routines for:

- **Loading Terrain Definitions:** Ensure classes responsible for terrain definitions have appropriate XML integration.
- **Patch Configuration:** XML should allow toggling patches without mod restart, leveraging IToggleablePatch interfaces.
- **Mod Settings Handling:** Persist settings and configurations using XML-backed ModSettings.

## Harmony Patching

Harmony patches are extensively used to alter game behavior subtly:

- **Identifying Patch Targets:** Use descriptive class and method names to target correct RimWorld assemblies.
- **Apply and Remove Patches Dynamically:** Implement methods in `ToggleablePatch` and `ToggleablePatchExtensions` for applying and removing patches as per player settings.
- **Patch Safety:** Ensure patches do not interfere with fundamental gameplay loops or cause instability in multiplayer environments.

## Suggestions for Copilot

When generating code with Copilot for this mod, incorporate the following suggestions:

- **Designator and Job Drivers:** Focus on classes like `Designator_Dig` and `JobDriver_Dig` for expanding digging operations.
- **Resource Management Systems:** Provide code that enhances or extends the `Blueprint_Build` and `Frame` systems for resource calculations.
- **Testing and Debugging Aids:** Enhance the `Utilities` class for common utility methods that can be reused across multiple mod components.
- **Interoperability Extensions:** Consider existing classes like `TerrainSystemOverhaul_Interop` and `WaterFreezes_Interop` for potential mod interops.

This guide should assist GitHub Copilot in generating useful, consistent, and maintainable code for maintaining and expanding this RimWorld mod. If you encounter concepts or sections that might confuse Copilot, provide examples or more detailed guidance.
