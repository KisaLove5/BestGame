using System.Collections.Generic;

namespace StrategyGame.Formations
{
    public class ThreeLineFormation : IFormationStrategy
    {
        public string Name => "3 линии";

        public IReadOnlyList<IReadOnlyList<Unit>> Arrange(IReadOnlyList<Unit> units)
        {
            var l1 = new List<Unit>();
            var l2 = new List<Unit>();
            var l3 = new List<Unit>();
            for (int i = 0; i < units.Count; i++)
            {
                int m = i % 3;
                if (m == 0) l1.Add(units[i]);
                else if (m == 1) l2.Add(units[i]);
                else l3.Add(units[i]);
            }
            return new List<IReadOnlyList<Unit>> { l1, l2, l3 };
        }
    }
}