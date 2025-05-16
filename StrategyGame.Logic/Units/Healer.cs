using System.Text;

namespace StrategyGame
{
    public class Healer : Unit
    {
        private int healCounter = 0;
        private int healCooldown = 4;

        public Healer()
        {
            maxHealth = 4;
            health = 4;
            attack = 1;
            defense = 0;
            cost = 80;
            range = 1;
            DisplayName = "Healer";
        }

        public override void DoPersonalAction(Army myArmy, Army enemyArmy, StringBuilder sb)
        {
            if (Health <= 0) return;   // мёртвый — ничего не делает
            if (!TryPerform(sb, "[Healer]", () =>
            {
                healCounter++;
                if (healCounter >= healCooldown)
                {
                    healCounter = 0;
                    sb.AppendLine("[Healer] Лечит союзников поблизости!");
                    HealAround(myArmy, sb);
                }
                else
                {
                    // Можно либо атаковать, либо пропускать
                    sb.AppendLine("[Healer] Слабо атакует (или пропускает).");
                    base.Attack(myArmy, enemyArmy);
                }
            }))
            {
                return;
            }
        }

        private void HealAround(Army ownArmy, StringBuilder sb)
        {
            var all = ownArmy.GetAllUnits();
            int idx = all.IndexOf(this);

            if (idx != -1)
            {
                // Полечим того, кто стоит "спереди" (idx-1)
                if (idx - 1 >= 0)
                {
                    var frontAlly = all[idx - 1] as Interfaces.IsHealable;
                    if (frontAlly != null)
                    {
                        frontAlly.ReceiveHeal(2);
                        sb.AppendLine("  [Healer] Полечил союзника спереди!");
                    }
                }
                // Полечим того, кто "сзади" (idx+1)
                if (idx + 1 < all.Count)
                {
                    var backAlly = all[idx + 1] as Interfaces.IsHealable;
                    if (backAlly != null)
                    {
                        backAlly.ReceiveHeal(2);
                        sb.AppendLine("  [Healer] Полечил союзника сзади!");
                    }
                }
            }
        }
    }
}
