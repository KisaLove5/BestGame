namespace StrategyGame.Decorators
{
    public class SharpnessDecorator : UnitDecorator
    {
        private int duration = 3;

        public SharpnessDecorator(Unit unit) : base(unit) { }

        public override int AttackValue => unit.AttackValue + 1;

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
