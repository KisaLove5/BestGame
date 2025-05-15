using System.Text;
using System.Text.Json;
using System.IO;
using StrategyGame.Serialization;

namespace StrategyGame
{
    public class Game
    {
        private Battle battle;

        public Army Army1 => battle?.Army1;
        public Army Army2 => battle?.Army2;

        public void SetupBattle(Army army1, Army army2)
        {
            battle = new Battle(army1, army2);
        }

        /// <summary>
        /// Выполнить один пошаговый ход боя. Возвращает лог за этот ход.
        /// </summary>
        public string DoOneTurn()
        {
            if (battle == null)
                return "Бой не создан.\r\n";

            return battle.DoOneTurn();
        }

        public bool IsBattleOver()
        {
            if (battle == null) return true;
            return battle.IsBattleOver;
        }

        public string GetFinalResult()
        {
            if (battle == null) return "Бой не создан.";
            return battle.GetFinalResult();
        }

        public string GetArmiesStatus()
        {
            if (battle == null) return "Бой не создан.";
            return battle.PrintStatus();
        }

        /// <summary>Сохраняем текущую битву в JSON-файл.</summary>
        public void Save(string path)
        {
            if (battle == null) throw new InvalidOperationException("Бой ещё не создан.");

            var dto = new SaveGameData
            {
                Army1 = Army1.GetAllUnits()
                             .Select(u => new UnitData { Type = u.GetType().Name, Hp = u.Health })
                             .ToList(),
                Army2 = Army2.GetAllUnits()
                             .Select(u => new UnitData { Type = u.GetType().Name, Hp = u.Health })
                             .ToList()
            };

            File.WriteAllText(path,
                JsonSerializer.Serialize(dto, new JsonSerializerOptions { WriteIndented = true }));
        }

        /// <summary>Статическая фабрика: загружает JSON, пересобирает армии и возвращает готовую Game.</summary>
        public static Game Load(string path, UnitFactory factory)
        {
            var dto = JsonSerializer.Deserialize<SaveGameData>(File.ReadAllText(path))
                      ?? throw new Exception("Не удалось разобрать сохранение.");

            Army Rebuild(List<UnitData> list)
            {
                var army = new Army();
                foreach (var ud in list)
                {
                    var unit = factory.CreateUnit(ud.Type);   // ваш существующий фабричный метод
                    unit.RestoreHp(ud.Hp);
                    army.AddUnit(unit);
                }
                return army;
            }

            var g = new Game();
            g.SetupBattle(Rebuild(dto.Army1), Rebuild(dto.Army2));
            return g;
        }

        // ===== снимок текущего состояния =====
        public SaveGameData GetSnapshot()
        {
            return new SaveGameData
            {
                Army1 = Army1.GetAllUnits()
                             .Select(u => new UnitData { Type = u.BaseTypeName, Hp = u.Health })
                             .ToList(),
                Army2 = Army2.GetAllUnits()
                             .Select(u => new UnitData { Type = u.BaseTypeName, Hp = u.Health })
                             .ToList()
            };
        }

        // ===== восстановление из снимка =====
        public void RestoreSnapshot(SaveGameData dto, UnitFactory factory)
        {
            Army Rebuild(List<UnitData> list)
            {
                var army = new Army();
                foreach (var ud in list)
                {
                    var unit = factory.CreateUnit(ud.Type);
                    unit.RestoreHp(ud.Hp);
                    army.AddUnit(unit);
                }
                return army;
            }
            SetupBattle(Rebuild(dto.Army1), Rebuild(dto.Army2));
        }

    }
}
