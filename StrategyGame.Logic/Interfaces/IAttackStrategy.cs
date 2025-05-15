namespace StrategyGame.Interfaces
{
    public interface IAttackStrategy
    {
        void ExecuteAttack(Unit attacker, Army myArmy, Army enemyArmy);
    }
}
