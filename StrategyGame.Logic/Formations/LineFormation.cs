using System.Collections.Generic;
using System.Linq;

namespace StrategyGame.Formations
{
    public class LineFormation : IFormationStrategy
    {
        public string Name => "Линия";

        public IReadOnlyList<IReadOnlyList<Unit>> Arrange(IReadOnlyList<Unit> units)
        {
            return new List<IReadOnlyList<Unit>> { units.ToList() };
        }
    }
}