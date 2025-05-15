using System;
using System.IO;
using System.Text;

namespace StrategyGame.Proxies
{
    public class AbilityLoggingProxy : Unit
    {
        private readonly Unit realUnit;
        private const string LogFile = "abilities.log";

        public AbilityLoggingProxy(Unit realUnit)
        {
            this.realUnit = realUnit;
            CopyStats(realUnit);
        }

        private void CopyStats(Unit unit)
        {
            this.maxHealth = unit.MaxHealth;
            this.health = unit.Health;
            this.attack = unit.AttackValue;
            this.defense = unit.Defense;
            this.cost = unit.Cost;
            this.range = unit.Range;
        }
        public override string DisplayName => realUnit.DisplayName;
        public override int Health => realUnit.Health;

        public override void Attack(Army myArmy, Army enemyArmy)
        {
            realUnit.Attack(myArmy, enemyArmy);
        }

        public override void ReceiveDamage(int damage)
        {
            realUnit.ReceiveDamage(damage);
        }

        public override void DoPersonalAction(Army myArmy, Army enemyArmy, StringBuilder sb)
        {
            int beforeLength = sb.Length;

            realUnit.DoPersonalAction(myArmy, enemyArmy, sb);

            if (sb.Length > beforeLength)
            {
                var actionText = sb.ToString(beforeLength, sb.Length - beforeLength);
                File.AppendAllText(LogFile,
                    $"{DateTime.Now}: {realUnit.GetType().Name} применил способность:{Environment.NewLine}{actionText}{Environment.NewLine}");
            }
        }
    }
}
