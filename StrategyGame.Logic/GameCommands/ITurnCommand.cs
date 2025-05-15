namespace StrategyGame.GameCommands
{
    public interface ITurnCommand
    {
        void Execute(Game game);
        void Undo(Game game);   // вернуть состояние «до»
        void Redo(Game game);   // применить состояние «после»
    }
}