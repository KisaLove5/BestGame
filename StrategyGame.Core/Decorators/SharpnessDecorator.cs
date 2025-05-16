namespace StrategyGame.Decorators
{
    public class SharpnessDecorator : UnitDecorator
    {
        private int duration = 3;

        public SharpnessDecorator(Unit unit) : base(unit) { }

        public override int AttackValue => Inner.AttackValue + 1;

        public override Unit Tick()
        {
            Inner = Inner is UnitDecorator decorator ? decorator.Tick() : Inner;

            duration--;
            if (duration <= 0)
            {
                return Inner; // снимаем декоратор
            }

            return this;
        }
    }
}
