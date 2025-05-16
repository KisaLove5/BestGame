using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StrategyGame.Interfaces;

namespace StrategyGame
{
    public class Battle
    {
        private Army army1;
        private Army army2;
        public bool IsBattleOver { get; private set; }

        public Army Army1 => army1;
        public Army Army2 => army2;

        public Battle(Army a1, Army a2)
        {
            army1 = a1;
            army2 = a2;
            IsBattleOver = false;
        }

        public string DoOneTurn()
        {
            if (IsBattleOver)
                return "Бой уже завершён.\r\n";

            var sb = new StringBuilder();

            // Ход первой армии
            ProcessArmyTurn(army1, army2, sb);
            if (CheckEnd(sb))
                return sb.ToString();

            // Ход второй армии
            ProcessArmyTurn(army2, army1, sb);
            if (CheckEnd(sb))
                return sb.ToString();

            // Если битва ещё не закончилась — возвращаем ход
            return sb.ToString();
        }

        public string GetFinalResult()
        {
            if (!IsBattleOver)
                return "Бой ещё идёт...";

            if (army1.IsDefeated() && !army2.IsDefeated())
                return "Армия2 победила!";
            else if (!army1.IsDefeated() && army2.IsDefeated())
                return "Армия1 победила!";
            else
                return "Ничья, обе армии уничтожены.";
        }

        public string PrintStatus()
        {
            var sb = new StringBuilder();
            sb.AppendLine("----- Статус армий -----");
            sb.AppendLine("Армия1:");
            foreach (var unit in army1.GetAllUnits())
            {
                sb.AppendLine($"  {unit.GetType().Name} (HP: {unit.Health})");
            }
            sb.AppendLine();
            sb.AppendLine("Армия2:");
            foreach (var unit in army2.GetAllUnits())
            {
                sb.AppendLine($"  {unit.GetType().Name} (HP: {unit.Health})");
            }
            sb.AppendLine("------------------------");
            return sb.ToString();
        }

        private void ProcessArmyTurn(Army active, Army enemy, StringBuilder sb)
        {
            var actLines = active.GetLines();
            var enLines = enemy.GetLines();

            int max = Math.Max(actLines.Count, enLines.Count);
            for (int line = 0; line < max; line++)
            {
                var own = line < actLines.Count ? actLines[line] : Array.Empty<Unit>();
                var foe = line < enLines.Count ? enLines[line] : Array.Empty<Unit>();

                // Берём «снимок» текущей линии, чтобы безопасно итерировать
                var snapshot = new List<Unit>(own);
                foreach (var unit in snapshot)
                {
                    // Если юнит уже мёртв или убран — пропускаем
                    if (!active.GetAllUnits().Contains(unit))
                        continue;

                    bool isFront = own.Count > 0 && own[0] == unit;
                    if (unit is IsFrontman && !isFront)
                        continue;

                    unit.DoPersonalAction(active, enemy, sb);
                    active.RemoveDeadUnits();
                    enemy.RemoveDeadUnits();

                    if (active.IsDefeated() || enemy.IsDefeated())
                        return;
                }
            }
        }

        private bool CheckEnd(StringBuilder sb)
        {
            if (army1.IsDefeated() || army2.IsDefeated())
            {
                IsBattleOver = true;
                sb.AppendLine(GetFinalResult());
                return true;
            }
            return false;
        }
    }
}
