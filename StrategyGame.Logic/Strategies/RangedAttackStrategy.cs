using StrategyGame.Interfaces;

namespace StrategyGame.Strategies
{
    public class RangedAttackStrategy : IAttackStrategy
    {
        public void ExecuteAttack(Unit attacker, Army myArmy, Army enemyArmy)
        {
            var enemyUnits = enemyArmy.GetAllUnits();
            int maxIndex = Math.Min(attacker.Range, enemyUnits.Count);
            if (maxIndex <= 0) return;

            var random = new Random();
            int targetIndex = random.Next(0, maxIndex);

            enemyUnits[targetIndex].ReceiveDamage(attacker.AttackValue);
        }
    }
}
