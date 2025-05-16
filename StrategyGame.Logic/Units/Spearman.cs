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
            if (Health <= 0) return;   // мёртвый — ничего не делает
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


        public override void Attack(Army myArmy, Army enemyArmy)
        {
            var enemyUnits = enemyArmy.GetAllUnits();
            int maxIndex = Math.Min(range, enemyUnits.Count);
            for (int i = 0; i < maxIndex; i++)
            {
                enemyUnits[i].ReceiveDamage(AttackValue);
            }
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

            };
        }
    }
}