using System.Text;
using StrategyGame.Interfaces;

namespace StrategyGame
{
    public class Swordsman : Unit, IsClonable, IsFrontman, IsHealable, IsBuffable
    {
        public Swordsman()
        {
            maxHealth = 12;
            health = 12;
            attack = 3;
            defense = 0;
            cost = 50;
            range = 1;
            DisplayName = "Swordsman";
        }

        public override void DoPersonalAction(Army myArmy, Army enemyArmy, StringBuilder sb)
        {
            if (Health <= 0) return;   // мёртвый — ничего не делает
            if (!TryPerform(sb, "[Swordsman]", () =>
            {
                sb.AppendLine("[Swordsman] Атакует врага (т.к. фронтовой)!");
                base.Attack(myArmy, enemyArmy);
            }))
            {
                return;
            }
        }

        public virtual void ReceiveHeal(int amount)
        {
            health += amount;
        }

        public Unit Clone()
        {
            return new Swordsman
            {
                health = this.health,
                attack = this.attack,
                defense = this.defense,
                cost = this.cost,
                range = this.range,
            };
        }
    }
}
