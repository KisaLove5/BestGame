using StrategyGame.Interfaces;
using System.Text;

namespace StrategyGame.Decorators
{
    public abstract class UnitDecorator : Unit
    {
        protected Unit Inner { get; private set; }
        protected UnitDecorator(Unit inner) : base(inner.Rnd) // унаследуем rnd
        {
            Inner = inner;
            UnitId = inner.UnitId;
        }

        private readonly int unitId;

        public override int UnitId => unitId;

        public override int Health => Inner.Health;
        public override int MaxHealth => Inner.MaxHealth;
        public override int AttackValue => Inner.AttackValue;
        public override int Defense => Inner.Defense;
        public override int Cost => Inner.Cost;
        public override int Range => Inner.Range;
        public override string DisplayName => Inner.DisplayName;

        public bool HasDecorator<T>() where T : Unit
        {
            if (this is T)
                return true;

            if (Inner is UnitDecorator decorator)
                return decorator.HasDecorator<T>();

            return false;
        }

        public override void Attack(Army myArmy, Army enemyArmy)
        {
            Inner.Attack(myArmy, enemyArmy);
        }

        public override void ReceiveDamage(int damage)
        {
            Inner.ReceiveDamage(damage);
        }

        public override void DoPersonalAction(Army myArmy, Army enemyArmy, StringBuilder sb)
        {
            Inner.DoPersonalAction(myArmy, enemyArmy, sb);
        }

        public override Unit Tick()
        {
            if (Inner is UnitDecorator decorator)
            {
                Inner = decorator.Tick();
            }

            return this;
        }

        public override Unit Clone() => (Unit)MemberwiseClone();
    }
}
