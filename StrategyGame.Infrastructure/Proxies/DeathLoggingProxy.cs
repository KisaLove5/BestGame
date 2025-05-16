using System;
using System.IO;
using System.Text;

namespace StrategyGame.Proxies
{
    public class DeathLoggingProxy : Unit
    {
        private readonly Unit realUnit;
        private const string LogFile = "deaths.log";

        public DeathLoggingProxy(Unit realUnit)
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
            int before = realUnit.Health;
            realUnit.ReceiveDamage(damage);
            int after = realUnit.Health;

            if (before > 0 && after <= 0)
            {
                File.AppendAllText(LogFile,
                    $"{DateTime.Now}: {realUnit.GetType().Name} погиб (HP: {before} -> {after}){Environment.NewLine}");
            }
        }

        public override void DoPersonalAction(Army myArmy, Army enemyArmy, StringBuilder sb)
        {
            realUnit.DoPersonalAction(myArmy, enemyArmy, sb);
        }
    }
}
