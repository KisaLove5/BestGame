using System.Collections.Generic;

namespace StrategyGame.GameCommands
{
    public class TurnInvoker
    {
        private readonly Stack<ITurnCommand> _undo = new();
        private readonly Stack<ITurnCommand> _redo = new();

        public void DoTurn(ITurnCommand cmd, Game game)
        {
            cmd.Execute(game);
            _undo.Push(cmd);
            _redo.Clear();
        }

        public void Undo(Game game)
        {
            if (_undo.Count == 0) return;
            var cmd = _undo.Pop();
            cmd.Undo(game);
            _redo.Push(cmd);
        }

        public string? Redo(Game game)
        {
            if (_redo.Count > 0)
            {
                var cmd = _redo.Pop();
                cmd.Redo(game);
                _undo.Push(cmd);
                return cmd is TurnCommand tc ? tc.TurnLog : null;
            }
            else if (_undo.Count > 0)
            {
                var last = _undo.Peek();
                last.Redo(game);
                return last is TurnCommand tc ? tc.TurnLog : null;
            }
            return null;
        }

        public void Clear()
        {
            _undo.Clear();
            _redo.Clear();
        }
    }
}