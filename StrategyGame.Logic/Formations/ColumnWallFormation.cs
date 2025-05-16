using System.Collections.Generic;

namespace StrategyGame.Formations
{
    public class ColumnWallFormation : IFormationStrategy
    {
        public string Name => "Стенка";

        public IReadOnlyList<IReadOnlyList<Unit>> Arrange(IReadOnlyList<Unit> units)
        {
            var lines = new List<IReadOnlyList<Unit>>();
            foreach (var u in units)
                lines.Add(new List<Unit> { u });
            return lines;
        }
    }
}