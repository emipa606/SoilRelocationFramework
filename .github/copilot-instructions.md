# GitHub Copilot Instructions: Soil Relocation Framework (Continued)

Welcome to the development guide for the Soil Relocation Framework (Continued) mod for RimWorld. This guide will provide you with an overview of the mod’s purpose, key features, coding patterns, XML integrations, and harmony patching. Additionally, it includes suggestions to leverage GitHub Copilot effectively for this project.

## Mod Overview and Purpose

The Soil Relocation Framework (Continued) mod allows players to transform and transport soil in RimWorld. It provides players with a "Dig" order to create resource items that can be placed back as terrain tiles, mimicking the behavior and attributes of original terrain. This mod facilitates advanced terrain management techniques, including farming, constructing greenhouses, and fire prevention through strategic soil relocation.

## Key Features and Systems

- **Dig Order**: Designate terrain for excavation to acquire soil resource items.
- **Terrain Replication**: Use acquired soil to recreate terrain accurately in desired locations such as the Floors menu.
- **Agricultural Optimization**: Relocate fertile soil for optimal farming conditions.
- **Terrain Modification**: Fill water cells and wet bridgeable cells with soil to customize map layouts.
- **Fire Prevention**: Remove soil to create firebreaks and control wildfires.
- **Recipes**: Convert stone into sand for additional construction options.
- **Optional Patches**: Toggleable patches to modify specific game/resource balance elements.

## Coding Patterns and Conventions

- **Folder Structure**: Organize scripts and XML by functional aspect, e.g., `SR/ToggleablePatches` for patches, `SR/DesignationDefOf` for designations.
- **Naming Conventions**: Use clear, descriptive names for classes and members, ensuring the code is understandable and maintainable.
- **Modular Code**: Break down functionality into separate classes, each handling distinct features.
- **XML Definitions**: Maintain XML files for game definitions like designations, jobs, and resources, ensuring compatibility with RimWorld’s requirement for custom mods.

## XML Integration

- **Designation Dropdowns**: Defined in `DesignationDropdownGroups.xml` to create hierarchy in the UI.
- **Job and Recipe Definitions**: Use `Jobs.xml` and `Recipes.xml` to introduce dig tasks and item conversions.
- **Resource Definitions**: Within `Resources.xml`, define the sandbox of new items such as soil and sand resources.

## Harmony Patching

- **Harmonizing Changes**: Use Harmony library to patch and extend core game functionality without directly altering its code, ensuring better compatibility.
- **Patch Management**: Organize patches in files like `DubsSkylights.cs`, `Vanilla.cs`, and `VFEArchitect.cs` for category-specific patches.
- **Toggleable Patches**: Use configuration settings allowing players to enable or disable individual patches.

## Suggestions for Copilot

- **Code Compliance**: Prompt Copilot to adhere to coding standards by providing explicit examples of the mod’s coding conventions.
- **XML Definitions**: Guide Copilot by illustrating the specific format and attributes required in XML files for RimWorld.
- **Variable Names**: Utilize Copilot to auto-generate descriptive variable names while maintaining context relevance.
- **Pattern Recognition**: Leverage Copilot to recognize patterns in harmony patches, suggesting similar structures for new patches.
- **Documentation Assistance**: Ask Copilot to draft comments and documentation within the code, enhancing clarity and assisting future development.

By following these instructions, contributors can work efficiently on the Soil Relocation Framework (Continued) mod. This will ensure code consistency and allow them to harness GitHub Copilot as a valuable tool in the development process. Happy coding!

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).

