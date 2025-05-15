using StrategyGame.Interfaces;
using System.Text;

namespace StrategyGame.Decorators
{
    public abstract class UnitDecorator : Unit
    {
        public Unit unit;
        private readonly int unitId;

        public UnitDecorator(Unit unit)
        {
            this.unit = unit;
            this.unitId = unit.UnitId; // сохраняем ID базового юнита
        }

        public override int UnitId => unitId;

        public override int Health => unit.Health;
        public override int MaxHealth => unit.MaxHealth;
        public override int AttackValue => unit.AttackValue;
        public override int Defense => unit.Defense;
        public override int Cost => unit.Cost;
        public override int Range => unit.Range;
        public override string DisplayName => unit.DisplayName;

        public bool HasDecorator<T>() where T : Unit
        {
            if (this is T)
                return true;

            if (unit is UnitDecorator decorator)
                return decorator.HasDecorator<T>();

            return false;
        }

        public override void Attack(Army myArmy, Army enemyArmy)
        {
            unit.Attack(myArmy, enemyArmy);
        }

        public override void ReceiveDamage(int damage)
        {
            unit.ReceiveDamage(damage);
        }

        public override void DoPersonalAction(Army myArmy, Army enemyArmy, StringBuilder sb)
        {
            unit.DoPersonalAction(myArmy, enemyArmy, sb);
        }

        public virtual Unit Tick()
        {
            if (unit is UnitDecorator decorator)
            {
                unit = decorator.Tick();
            }

            return this;
        }
    }
}
