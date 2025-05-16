using System;
using System.Collections.Generic;

namespace StrategyGame
{
    /// <summary>Утилита для генерации случайной армии на заданный бюджет.</summary>
    public static class RandomArmyGenerator
    {
        private static readonly Random rnd = new();

        public static Army Generate(int budget, IArmyFactory factory, IRandomService rnd)
        {
            // Список (тип, cost) — если добавите новых юнитов, просто расширьте.
            var catalog = new (string type, int cost)[]
            {
                ("Swordsman",    50),
                ("Spearman",     60),
                ("Archer",       40),
                ("Mage",        100),
                ("Healer",       80),
                ("WeaponBearer", 70),
                ("Wall",         60)
            };

            int minCost = int.MaxValue;
            foreach (var (_, c) in catalog) minCost = Math.Min(minCost, c);

            var army = new Army();

            while (budget >= minCost)
            {
                // получаем список доступных по оставшемуся бюджету
                var options = new List<(string type, int cost)>();
                foreach (var item in catalog)
                    if (item.cost <= budget) options.Add(item);

                if (options.Count == 0) break;            // на остаток не хватает ни одного юнита

                var choice = options[rnd.Next(options.Count)];
                var unit = factory.CreateUnit(choice.type);

                if (unit == null)                       // вдруг забыли тип в фабрике
                    throw new InvalidOperationException($"Factory can't build {choice.type}");

                army.AddUnit(unit);
                budget -= choice.cost;
            }

            return army;
        }
    }
}
