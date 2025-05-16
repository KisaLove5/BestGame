using System.Text;
using StrategyGame.Interfaces;

namespace StrategyGame
{
    public class Battle
    {
        public Army Army1 { get; }
        public Army Army2 { get; }
        public bool IsOver { get; private set; }

        public Battle(Army a1, Army a2) { Army1 = a1; Army2 = a2; }

        /// <summary>
        /// За один ход перебираем ВСЕ юниты армии1, потом ВСЕ юниты армии2.
        /// Но если юнит реализует IsFrontman (мечник/копейщик)
        /// и при этом не стоит спереди — он пропускает ход.
        /// </summary>
        public string Turn()
        {
            if (IsOver) return "Бой завершён.\r\n";
            var sb = new StringBuilder();
            ProcessArmy(Army1, Army2, sb);
            ProcessArmy(Army2, Army1, sb);
            return sb.ToString();

            void ProcessArmy(Army my, Army enemy, StringBuilder log)
            {
                foreach (var unit in my.Units.ToList()) // snapshot, т.к. могут умирать
                {
                    if (!my.Units.Contains(unit)) continue;      // уже мёртв
                    if (unit is IsFrontman && my.Front() != unit) continue;

                    unit.DoPersonalAction(my, enemy, log);
                    my.RemoveDead(); enemy.RemoveDead();

                    if (my.IsDefeated() || enemy.IsDefeated())
                    { IsOver = true; break; }
                }
            }
        }

        public string Result() => !IsOver
            ? "Бой ещё идёт..."
            : Army1.IsDefeated() == Army2.IsDefeated() ? "Ничья"
              : Army1.IsDefeated() ? "Армия2 победила" : "Армия1 победила";
    }
}
