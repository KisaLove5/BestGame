namespace StrategyGame.Interfaces
{
    // Фабрика для создания юнитов
    public interface IUnitFactory
    {
        Unit CreateUnit(string type);
    }
}
