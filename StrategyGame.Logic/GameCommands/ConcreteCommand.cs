using StrategyGame.Serialization;

namespace StrategyGame.GameCommands
{
    public class TurnCommand : ITurnCommand
    {
        private SaveGameData? _before, _after;
        public string TurnLog { get; private set; } = "";

        public void Execute(Game game)
        {
            var factory = new UnitFactory();
            _before = game.GetSnapshot();
            TurnLog = game.DoOneTurn();// снимок ДО                  // сам ход
            _after = game.GetSnapshot();       // снимок ПОСЛЕ
        }

        public void Undo(Game game)
        {
            if (_before == null) return;
            var factory = new UnitFactory();
            game.RestoreSnapshot(_before, factory);
        }
        public void Redo(Game game)
        {
            if (_after == null) return;
            var factory = new UnitFactory();
            game.RestoreSnapshot(_after, factory);
        }
    }
}
