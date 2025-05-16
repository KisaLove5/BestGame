namespace StrategyGame.Decorators
{
    public class PotionDecorator : UnitDecorator
    {
        private int duration = 3;

        public PotionDecorator(Unit unit) : base(unit) { }

        public override int AttackValue => Inner.AttackValue + 2;

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
