using StrategyGame.Abstractions;
using StrategyGame;

public interface IUnitBuilder
{
    string TypeId { get; }
    Unit Create(IRandomService rnd);
}