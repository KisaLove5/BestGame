using StrategyGame;

public interface IArmyFactory
{
    Unit Create(string typeId);            // остаётся совместимой сигнатурой
}