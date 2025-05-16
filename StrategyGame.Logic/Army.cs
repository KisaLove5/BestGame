using System.Collections.Generic;

namespace StrategyGame
{
    public class Army
    {
        private List<Unit> units = new List<Unit>();

        // ---------- стратегия построения ----------
        private StrategyGame.Formations.IFormationStrategy formation =
            new StrategyGame.Formations.LineFormation();

        public StrategyGame.Formations.IFormationStrategy Formation => formation;

        public void SetFormation(StrategyGame.Formations.IFormationStrategy newFormation)
        {
            formation = newFormation ?? throw new System.ArgumentNullException(nameof(newFormation));
        }

        public System.Collections.Generic.IReadOnlyList<System.Collections.Generic.IReadOnlyList<Unit>> GetLines()
            => formation.Arrange(units);

        public void AddUnit(Unit unit)
        {
            units.Add(unit);
        }

        public void RemoveUnit(Unit unit)
        {
            units.Remove(unit);
        }

        public Unit GetFrontUnit()
        {
            // нужен первый ЖИВОЙ юнит, а не просто первый в списке
            foreach (var u in units)
                   if (u.Health > 0) return u;
            return null;
        }

        public bool IsDefeated()
        {
            return units.Count == 0;
        }

        public List<Unit> GetAllUnits()
        {
            return units;
        }

        public void MoveUnit(Unit unit, int newIndex, int flag)
        {
            int oldIndex = units.IndexOf(unit);
            if (oldIndex == -1) return;
            if (newIndex < 0 || newIndex >= units.Count) return;

            // Убираем из старой позиции и вставляем в новую
            units.RemoveAt(oldIndex);
            if (flag == 1)
            {
                if (newIndex > oldIndex) newIndex--;
            }
            else
            {
                if (newIndex < oldIndex) newIndex++;
            }
            units.Insert(newIndex, unit);
        }

        public void RemoveDeadUnits()
        {
            units.RemoveAll(u => u.Health <= 0);
        }
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
