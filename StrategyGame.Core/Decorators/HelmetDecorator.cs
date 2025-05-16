namespace StrategyGame.Decorators
{
    public class HelmetDecorator : UnitDecorator
    {
        private int armorHp = 3;

        public HelmetDecorator(Unit unit) : base(unit) { }

        public override int Defense => Inner.Defense + (armorHp > 0 ? 1 : 0);

        public override void ReceiveDamage(int damage)
        {
            if (armorHp > 0)
            {
                armorHp -= damage;
                if (armorHp < 0) armorHp = 0;
            }
            else
            {
                Inner.ReceiveDamage(damage);
            }
        }

        public override Unit Tick()
        {
            Inner = Inner is UnitDecorator decorator ? decorator.Tick() : Inner;

            if (armorHp <= 0)
            {
                return Inner; // снимаем декоратор
            }

            return this;
        }
    }
}
