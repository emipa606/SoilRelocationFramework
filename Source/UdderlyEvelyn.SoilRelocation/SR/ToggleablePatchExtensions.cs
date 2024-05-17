namespace SR;

public static class ToggleablePatchExtensions
{
    public static void Process(this IToggleablePatch patch)
    {
        if (patch.Enabled)
        {
            patch.Apply();
        }
        else if (patch.Applied)
        {
            patch.Remove();
        }
        else
        {
            ToggleablePatch.MessageLoggingMethod(
                $"[ToggleablePatch] Skipping patch \"{patch.Name}\" because it is disabled.");
        }
    }
}