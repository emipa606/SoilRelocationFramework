namespace SR;

public interface IToggleablePatch
{
    string Name { get; }

    bool Enabled { get; }

    bool Applied { get; }

    void Apply();

    void Remove();
}