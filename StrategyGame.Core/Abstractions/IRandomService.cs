namespace StrategyGame.Abstractions;
public interface IRandomService
{
    double NextDouble();
    int Next(int minInclusive, int maxExclusive);
}
