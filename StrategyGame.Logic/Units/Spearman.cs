using System.Text;
using StrategyGame.Interfaces;

namespace StrategyGame
{
    public class Spearman : Unit, IsClonable, IsFrontman, IsHealable, IsBuffable
    {
        public Spearman()
        {
            maxHealth = 8;
            health = 8;
            attack = 2;
            defense = 0;
            cost = 60;
            range = 2;
            DisplayName = "Spearman";
        }

        public override void DoPersonalAction(Army myArmy, Army enemyArmy, StringBuilder sb)
        {
            if (!TryPerform(sb, "[Spearman  ]", () =>
            {
                sb.AppendLine("[Spearman] Атакует несколько целей (мульти-удар)!");
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
            return new Spearman
            {
                health = this.health,
                attack = this.attack,
                defense = this.defense,
                cost = this.cost,
                range = this.range,
                attackStrategy = this.attackStrategy
            };
        }
    }
}
