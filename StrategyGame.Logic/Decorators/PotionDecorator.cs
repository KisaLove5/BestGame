namespace StrategyGame.Decorators
{
    public class PotionDecorator : UnitDecorator
    {
        private int duration = 3;

        public PotionDecorator(Unit unit) : base(unit) { }

        public override int AttackValue => unit.AttackValue + 2;

        public override Unit Tick()
        {
            unit = unit is UnitDecorator decorator ? decorator.Tick() : unit;

            duration--;
            if (duration <= 0)
            {
                return unit; // снимаем декоратор
            }

            return this;
        }
    }
}
