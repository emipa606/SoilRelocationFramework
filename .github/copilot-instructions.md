# GitHub Copilot Instructions for Soil Relocation Framework (Continued)

## Mod Overview and Purpose
Soil Relocation Framework (Continued) is an update of Evelyn's mod for RimWorld. This mod introduces a "Dig" order that allows players to remove and relocate soil or other terrain resources. The resource items can then be used to place the terrain back down in the "Floors" menu, behaving precisely like the original terrain. This mod aims to enhance terrain manipulation and resource management, enabling players to create more strategic and efficient farms, greenhouses, and other structures.

## Key Features and Systems
### Terrain Manipulation
- **Dig Order:** Remove soil or other terrain resources and obtain resource items.
- **Replacement:** Use the resource items to replace terrain in the "Floors" menu.
- **Fertility Management:** Move fertile soil to create consistent farming areas.

### Optional Patches
- **Rebalance Vanilla:** Adjust recipes and terrain usage to improve balance.
- **Compatibility with Other Mods:** Optional patches for compatibility with various other mods.

### Utilities
- **Sand Recipes:** Recipes to grind stone into sand for sandbag creation.
- **Stony Soil:** Stony soil requires less material to fill water or wet bridgeable terrain.

## Coding Patterns and Conventions
- **Mod Structure:** The mod is structured with clear separation of concerns, including separate files for terrain manipulation, resource management, and optional patches.
- **Naming Conventions:** Use descriptive and consistent naming for classes, methods, and variables.
- **Comments and Documentation:** Provide detailed comments and XML documentation for clarity and ease of maintenance.

## XML Integration
- **Definitions:** XML files define designations, jobs, recipes, and resources.
- **Designation Dropdown Groups:** Customize the designations available in the game.
- **Jobs:** Define new jobs for soil manipulation.
- **Recipes:** Add new recipes for resource conversion.
- **Resources:** Define new or modified resource items.

## Harmony Patching
- **Harmony:** Utilize Harmony for patching Vanilla and other mods.
- **Toggleable Patches:** Optional patches that can be enabled or disabled through mod settings.
- **Compatibility Patches:** Adjust behavior to ensure compatibility with other mods.

## Suggestions for Copilot
1. **Terrain Manipulation:**
   - Implement the "Dig" order and resource item collection.
   - Develop the logic for replacing terrain with resource items.
   - Ensure consistent behavior with the original terrain.

2. **XML Definitions:**
   - Create and modify XML files for designations, jobs, recipes, and resources.
   - Ensure XML definitions are correctly integrated into the mod.

3. **Harmony Patches:**
   - Implement Harmony patches for rebalancing Vanilla features.
   - Create optional patches for compatibility with other mods.
   - Ensure patches are toggleable and non-invasive.

4. **Utilities:**
   - Develop recipes for converting stone into sand.
   - Implement special properties for stony soil.

5. **Mod Settings:**
   - Create a user-friendly interface for toggling optional patches.
   - Ensure mod settings are saved and loaded correctly.

6. **Testing and Debugging:**
   - Conduct thorough testing to ensure compatibility with Vanilla and other mods.
   - Debug any issues that arise during testing and provide detailed logs for troubleshooting.

By following these guidelines, you can contribute effectively to the development of the Soil Relocation Framework (Continued) and enhance the terrain manipulation capabilities of RimWorld.

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
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

