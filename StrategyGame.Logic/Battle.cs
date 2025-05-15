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

        /// <summary>
        /// За один ход перебираем ВСЕ юниты армии1, потом ВСЕ юниты армии2.
        /// Но если юнит реализует IsFrontman (мечник/копейщик)
        /// и при этом не стоит спереди — он пропускает ход.
        /// </summary>
        public string DoOneTurn()
        {
            if (IsBattleOver)
                return "Бой уже завершён.\r\n";

            var sb = new StringBuilder();

            // 1) Ход армии1
            var list1 = new List<Unit>(army1.GetAllUnits());
            foreach (var unit in list1)
            {
                if (!army1.GetAllUnits().Contains(unit))
                    continue; // на случай, если уже умер

                if (unit is IsFrontman)
                {
                    // Это мечник или копейщик, проверяем фронт
                    if (army1.GetFrontUnit() == unit)
                    {
                        unit.DoPersonalAction(army1, army2, sb);
                    }
                }
                else
                {
                    // (Archer, Mage, Healer) — ходят всегда
                    unit.DoPersonalAction(army1, army2, sb);
                }

                // Чистим мёртвых
                army1.RemoveDeadUnits();
                army2.RemoveDeadUnits();

                if (army1.IsDefeated() || army2.IsDefeated())
                {
                    IsBattleOver = true;
                    sb.AppendLine(GetFinalResult());
                    return sb.ToString();
                }
            }

            // 2) Ход армии2
            var list2 = new List<Unit>(army2.GetAllUnits());
            foreach (var unit in list2)
            {
                if (!army2.GetAllUnits().Contains(unit))
                    continue;

                if (unit is IsFrontman)
                {
                    if (army2.GetFrontUnit() == unit)
                    {
                        unit.DoPersonalAction(army2, army1, sb);
                    }
                }
                else
                {
                    unit.DoPersonalAction(army2, army1, sb);
                }

                army1.RemoveDeadUnits();
                army2.RemoveDeadUnits();

                if (army1.IsDefeated() || army2.IsDefeated())
                {
                    IsBattleOver = true;
                    sb.AppendLine(GetFinalResult());
                    return sb.ToString();
                }
            }

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
    }
}
