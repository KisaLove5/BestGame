using System.Text;

namespace StrategyGame
{
    public class Mage : Unit
    {
        private readonly Random rnd = new();
        private const int CopyCooldown = 3;  // каждые 3 личных хода
        private int turnsSinceCopy = 0;

        public Mage()
        {
            maxHealth = 4;
            health = 4;
            attack = 1;
            defense = 0;
            cost = 100;
            range = 1;
            DisplayName = "Mage";
        }

        public override void DoPersonalAction(Army myArmy, Army enemyArmy, StringBuilder sb)
        {
            if (Health <= 0) return;   // мёртвый — ничего не делает
            if (!TryPerform(sb, "[Mage]", () =>
            {
                turnsSinceCopy++;

                // если пришло время копировать
                if (turnsSinceCopy >= CopyCooldown)
                {
                    turnsSinceCopy = 0;
                    sb.AppendLine("[Mage] использует магию копирования!");
                    TryCopyUnit(myArmy, sb);
                }
                else
                {
                    sb.AppendLine("[Mage] наносит слабый удар!");
                    base.Attack(myArmy, enemyArmy);
                }
            }))
            {
                return;
            }
        }

        private void TryCopyUnit(Army ownArmy, StringBuilder sb)
        {
            // собираем ВСЕ Clonable (кроме самих магов и хилеров) — хоть спереди, хоть сзади
            var candidates = ownArmy.GetAllUnits().FindAll(
                u => u is Interfaces.IsClonable && !(u is Mage) && !(u is Healer));

            if (candidates.Count == 0)
            {
                sb.AppendLine("  • Нет подходящих целей.");
                return;
            }

            // выбираем случайную цель и клонируем
            var chosen = (Interfaces.IsClonable)candidates[rnd.Next(candidates.Count)];
            var clone = chosen.Clone();

            // ВСТАВЛЯЕМ КЛОН ПЕРЕД МАГОМ, сохраняя предыдущих клонов!
            ownArmy.InsertUnitBefore(this, clone);

            sb.AppendLine($"  • Создан клон {clone.DisplayName}. Армия сдвинута вперёд.");
        }
    }
}
