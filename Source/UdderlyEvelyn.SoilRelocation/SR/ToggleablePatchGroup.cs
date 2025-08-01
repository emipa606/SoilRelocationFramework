using System.Collections.Generic;

namespace SR;

public class ToggleablePatchGroup : IToggleablePatch
{
    public List<IToggleablePatch> Patches;

    public string Name { get; set; }

    public bool Enabled { get; set; }

    public bool Applied { get; private set; }

    public void Apply()
    {
        if (!Applied)
        {
            ToggleablePatch.MessageLoggingMethod($"[ToggleablePatch] Applying patches in patch group \"{Name}\"..");
            foreach (var patch in Patches)
            {
                patch.Apply();
            }

            Applied = true;
        }
        else
        {
            ToggleablePatch.MessageLoggingMethod(
                $"[ToggleablePatch] Skipping application of patch group \"{Name}\" because it is already applied.");
        }
    }

    public void Remove()
    {
        if (Applied)
        {
            ToggleablePatch.MessageLoggingMethod($"[ToggleablePatch] Removing patches in patch group \"{Name}\"..");
            foreach (var patch in Patches)
            {
                patch.Remove();
            }

            Applied = false;
        }
        else
        {
            ToggleablePatch.MessageLoggingMethod(
                $"[ToggleablePatch] Skipping removal of patch group \"{Name}\" because it is not applied.");
        }
    }
}