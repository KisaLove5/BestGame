using StrategyGame.Interfaces;

namespace StrategyGame.Strategies
{
    public class MeleeAttackStrategy : IAttackStrategy
    {
        public void ExecuteAttack(Unit attacker, Army myArmy, Army enemyArmy)
        {
            var frontUnit = enemyArmy.GetFrontUnit();
            if (frontUnit != null)
            {
                frontUnit.ReceiveDamage(attacker.AttackValue);
            }
        }
    }
}
