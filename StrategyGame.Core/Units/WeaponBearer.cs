using System;
using System.Collections.Generic;
using System.Text;
using StrategyGame.Decorators;
using StrategyGame.Interfaces;

namespace StrategyGame
{
    public class WeaponBearer : Unit
    {
        private int counter = 0;
        private int cooldown = 3;

        public WeaponBearer()
        {
            maxHealth = 6;
            health = 6;
            attack = 1;
            defense = 0;
            cost = 70;
            range = 1;
            DisplayName = "WeaponBearer";
        }

        public override void DoPersonalAction(Army myArmy, Army enemyArmy, StringBuilder sb)
        {
            if (!TryPerform(sb, "[WeaponBearer]", () =>
            {
                // Каждый ход уменьшаем ожидание, даже если на передовой
                counter++;

                var units = myArmy.GetAllUnits();
                int index = units.IndexOf(this);

                // Если юнит на передовой позиции – сразу атакуем
                if (index == 0)
                {
                    sb.AppendLine("[WeaponBearer] На передовой — атакует!");
                    base.Attack(myArmy, enemyArmy);
                    return;
                }

                // Проверяем готовность к баффу по таймеру
                if (counter < cooldown)
                {
                    sb.AppendLine("[WeaponBearer] Отдыхает...");
                    return;
                }

                // Сброс таймера и поиск союзников для баффа
                counter = 0;
                var targets = new List<Unit>();

                if (index > 0 && IsBuffableUnit(units[index - 1]))
                    targets.Add(units[index - 1]);

                if (index < units.Count - 1 && IsBuffableUnit(units[index + 1]))
                    targets.Add(units[index + 1]);

                if (targets.Count == 0)
                {
                    sb.AppendLine("[WeaponBearer] Нет подходящих союзников рядом.");
                    return;
                }

                var random = new Random();
                var target = targets[random.Next(targets.Count)];

                var buffs = new List<Func<Unit, Unit>>
                {
                    u => HasDecorator<HelmetDecorator>(u)    ? u : new HelmetDecorator(u),
                    u => HasDecorator<ShieldDecorator>(u)    ? u : new ShieldDecorator(u),
                    u => HasDecorator<PotionDecorator>(u)    ? u : new PotionDecorator(u),
                    u => HasDecorator<SharpnessDecorator>(u)? u : new SharpnessDecorator(u),
                };

                buffs.RemoveAll(f => f(target) == target);

                if (buffs.Count == 0)
                {
                    sb.AppendLine("[WeaponBearer] Все баффы уже наложены на " + target.DisplayName + ".");
                    return;
                }

                var applyBuff = buffs[random.Next(buffs.Count)];
                int targetIndex = myArmy.GetAllUnits().IndexOf(target);
                var newTarget = applyBuff(target);
                myArmy.GetAllUnits()[targetIndex] = newTarget;

                sb.AppendLine($"[WeaponBearer] Баффует {target.DisplayName} новым баффом: {newTarget.GetType().Name}!");
            }))
            {
                return;
            }
        }

        private bool IsBuffableUnit(Unit unit)
        {
            while (unit is UnitDecorator decorator)
                unit = decorator.unit;
            return unit is IsBuffable;
        }

        private bool HasDecorator<T>(Unit unit) where T : Unit
        {
            while (unit is UnitDecorator decorator)
            {
                if (decorator is T)
                    return true;
                unit = decorator.unit;
            }
            return false;
        }

        public override void Attack(Army myArmy, Army enemyArmy)
        {
            base.Attack(myArmy, enemyArmy);
        }
    }
}
