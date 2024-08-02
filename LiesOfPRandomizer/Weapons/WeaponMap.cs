namespace LiesOfPRandomizer;

public record class WeaponMap(IDictionary<string, WeaponMap.Handle> handles) : Module.Map<WeaponModule>
{
    public record struct Handle(string bladeName, string? motivity, string? technique, string? advance);

    public Handle this[string handleName] => handles[handleName];
    internal string GetBladeForHandle(string name)
    {
        if (handles.ContainsKey(name)) {
            return handles[name].bladeName;
        }
        return name.Replace("_HND_", "_BLD_");
    }

    internal bool Contains(string name)
    {
        return handles.ContainsKey(name);
    }
}
