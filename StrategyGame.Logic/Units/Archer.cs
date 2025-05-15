using System;
using System.Text;
using StrategyGame.Interfaces;

namespace StrategyGame
{
    public class Archer : Unit, IsClonable, IsHealable
    {
        public Archer()
        {
            maxHealth = 5;
            health = 5;
            attack = 4;
            defense = 0;
            cost = 40;
            range = 2;
            DisplayName = "Archer";
        }

        public override void DoPersonalAction(Army myArmy, Army enemyArmy, StringBuilder sb)
        {
            if (!TryPerform(sb, "[Archer]", () =>
            {
                int myIndex = myArmy.GetAllUnits().IndexOf(this);

                // Осталось «дистанции» до цели
                int distanceLeft = Range - myIndex;

                // Если впереди ≥ Range наших юнитов — цели недоступны
                if (distanceLeft <= 0)
                {
                    sb.AppendLine("[Archer] цели заслонены союзниками – пропуск хода.");
                    return;
                }

                var enemy = enemyArmy.GetAllUnits();
                if (enemy.Count == 0) return;

                // Стреляет по ПЕРВОЙ доступной цели, а не по всем подряд
                int targetIndex = Math.Min(distanceLeft - 1, enemy.Count - 1);
                var target = enemy[targetIndex];

                sb.AppendLine($"[Archer] стреляет в {target.DisplayName} (позиция {targetIndex}), " +
                              $"нанося {AttackValue} урона.");
                target.ReceiveDamage(AttackValue);
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
            return new Archer
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
