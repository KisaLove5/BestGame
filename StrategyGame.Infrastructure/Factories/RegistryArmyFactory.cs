public sealed class RegistryArmyFactory : IArmyFactory
{
    private readonly Dictionary<string, IUnitBuilder> _registry;

    public RegistryArmyFactory(IEnumerable<IUnitBuilder> builders)
        => _registry = builders.ToDictionary(b => b.TypeId, b => b,
                                             StringComparer.OrdinalIgnoreCase);

    public Unit Create(string typeId)
        => _registry.TryGetValue(typeId, out var b) ? b.Create(_rnd) : null;

    private readonly IRandomService _rnd;          // в конструктор DI
}