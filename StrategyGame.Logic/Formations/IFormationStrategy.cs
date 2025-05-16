using System.Collections.Generic;

namespace StrategyGame.Formations
{
    public interface IFormationStrategy
    {
        IReadOnlyList<IReadOnlyList<Unit>> Arrange(IReadOnlyList<Unit> units);
        string Name { get; }
    }
}