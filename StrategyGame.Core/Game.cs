using StrategyGame;

public sealed class Game
{
    public Battle? Current { get; private set; }

    public void Setup(Army a1, Army a2) => Current = new Battle(a1, a2);

    public string Turn() => Current?.Turn() ?? "Бой не создан.";
    public bool IsOver() => Current?.IsOver ?? true;
    public string Result() => Current?.Result() ?? "Бой не создан.";

    public BattleSnapshot Snapshot() =>
        Current == null
            ? new BattleSnapshot(Array.Empty<UnitSnapshot>(), Array.Empty<UnitSnapshot>())
            : new BattleSnapshot(
                Current.Army1.Units.Select(u => new UnitSnapshot(u.BaseTypeName, u.Health)).ToList(),
                Current.Army2.Units.Select(u => new UnitSnapshot(u.BaseTypeName, u.Health)).ToList());
}
