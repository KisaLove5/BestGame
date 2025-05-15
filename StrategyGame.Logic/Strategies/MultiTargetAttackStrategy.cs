using StrategyGame.Interfaces;

namespace StrategyGame.Strategies
{
    public class MultiTargetAttackStrategy : IAttackStrategy
    {
        public void ExecuteAttack(Unit attacker, Army myArmy, Army enemyArmy)
        {
            var enemyUnits = enemyArmy.GetAllUnits();
            int maxIndex = Math.Min(attacker.Range, enemyUnits.Count);
            for (int i = 0; i < maxIndex; i++)
            {
                enemyUnits[i].ReceiveDamage(attacker.AttackValue);
            }
        }
    }
}
