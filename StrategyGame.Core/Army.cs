using System.Collections.Generic;

namespace StrategyGame
{
    public class Army
    {
        private readonly List<Unit> units = new();
        public IReadOnlyList<Unit> Units => units;

        public void AddUnit(Unit unit)
        {
            units.Add(unit);
        }

        public void RemoveUnit(Unit unit)
        {
            units.Remove(unit);
        }

        public Unit? Front() => units.Count == 0 ? null : units[0];

        public bool IsDefeated() => units.Count == 0;

        public List<Unit> GetAllUnits()
        {
            return units;
        }

        public void Move(Unit u, int newIdx)
        {
            var old = units.IndexOf(u);
            if (old < 0 || newIdx < 0 || newIdx >= units.Count) return;
            units.RemoveAt(old);
            units.Insert(newIdx, u);
        }

        public void RemoveDead() => units.RemoveAll(x => x.Health <= 0);
        public void InsertUnitBefore(Unit existingUnit, Unit newUnit)
        {
            int index = units.IndexOf(existingUnit);
            if (index != -1)
            {
                units.Insert(index, newUnit);
            }
        }

        public void ReplaceUnit(Unit oldUnit, Unit newUnit)
        {
            int index = units.IndexOf(oldUnit);
            if (index != -1)
            {
                units[index] = newUnit;
            }
        }


    }
}
