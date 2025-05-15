namespace StrategyGame.Decorators
{
    public class HelmetDecorator : UnitDecorator
    {
        private int armorHp = 3;

        public HelmetDecorator(Unit unit) : base(unit) { }

        public override int Defense => unit.Defense + (armorHp > 0 ? 1 : 0);

        public override void ReceiveDamage(int damage)
        {
            if (armorHp > 0)
            {
                armorHp -= damage;
                if (armorHp < 0) armorHp = 0;
            }
            else
            {
                unit.ReceiveDamage(damage);
            }
        }

        public override Unit Tick()
        {
            unit = unit is UnitDecorator decorator ? decorator.Tick() : unit;

            if (armorHp <= 0)
            {
                return unit; // снимаем декоратор
            }

            return this;
        }
    }
}
