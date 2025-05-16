namespace StrategyGame;
public record UnitSnapshot(string Type, int Hp);

public record BattleSnapshot(
    IReadOnlyList<UnitSnapshot> Army1,
    IReadOnlyList<UnitSnapshot> Army2);