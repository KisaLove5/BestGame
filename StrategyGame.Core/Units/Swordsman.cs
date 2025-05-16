using System.Text;
using StrategyGame.Abstractions;
using StrategyGame.Interfaces;

namespace StrategyGame
{
    public class Swordsman : Unit, IsClonable, IsFrontman, IsHealable, IsBuffable
    {
        public Swordsman(IRandomService rnd) : base(rnd)
        {
            maxHealth = health = 12;
            attack = 3; defense = 0; cost = 50; range = 1;
        }

        public override void DoPersonalAction(Army my, Army enemy, StringBuilder sb)
        {
            if (!TryPerform(sb, "[Swordsman]", () =>
            {
                sb.AppendLine("[Swordsman] атакует врага!");
                Attack(my, enemy);
            })) return;
        }

        public virtual void ReceiveHeal(int amount)
        {
            health += amount;
        }

        public Unit Clone() => new Swordsman(Rnd) { health = this.health };
    }
}
