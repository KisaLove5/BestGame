using System;
using System.Text;

namespace StrategyGame
{
    public abstract class Unit
    {
        protected int maxHealth;
        protected int health;
        protected int attack;
        protected int defense;
        protected int cost;
        protected int range;

        private static int idCounter = 0;
        private readonly int unitId = idCounter++;

        public virtual int UnitId => unitId;

        public virtual int Health => health;
        public virtual int MaxHealth => maxHealth;
        public virtual int AttackValue => attack;
        public virtual int Defense => defense;
        public virtual int Cost => cost;
        public virtual int Range => range;

        protected string? displayName;
        public virtual string DisplayName
        {
            get => displayName ?? GetType().Name;
            set => displayName = value;
        }

        public virtual string BaseTypeName =>
            GetType().Name.EndsWith("Proxy") && GetType().BaseType != null
                ? GetType().BaseType!.Name
                : GetType().Name;

        private readonly Random _rnd = new();

        // --------------- Базовые штуки игрового движка (шанс на успех/промах) ----------------
        private const double ChanceDecay = 0.7; // после успеха
        private const double ChanceReset = 1.00;  // после провала
        private double _currentChance = 1.00;

        protected bool TryPerform(StringBuilder sb, string unitTag, Action doAction)
        {
            if (_rnd.NextDouble() <= _currentChance)
            {
                // УСПЕХ
                doAction();
                _currentChance = Math.Max(0.05, _currentChance * ChanceDecay);
                return true;
            }

            // ПРОМАХ
            sb.AppendLine($"{unitTag} промахивается имея шанс {Math.Round(_currentChance * 100, 1)} %");
            _currentChance = ChanceReset;
            return false;
        }

        // ------------------------------------------------------------------------------------

        /// <summary>
        /// Стандартная атака.
        /// </summary>
        public virtual void Attack(Army myArmy, Army enemyArmy)
        {
            if (enemyArmy == null || enemyArmy.IsDefeated()) return;

            var enemyUnits = enemyArmy.GetAllUnits();
            int maxIndex = Math.Min(range, enemyUnits.Count);
            if (maxIndex <= 0) return;

            if (range <= 1)
            {
                // Ближний бой — фронт
                var frontUnit = enemyArmy.GetFrontUnit();
                frontUnit?.ReceiveDamage(AttackValue);
            }
            else
            {
                // Стреляем по случайной цели из первых <range> позиций
                int targetIndex = _rnd.Next(0, maxIndex);
                enemyUnits[targetIndex].ReceiveDamage(AttackValue);
            }
        }

        public virtual void DoPersonalAction(Army myArmy, Army enemyArmy, StringBuilder sb)
        {
        }

        protected bool IsFront(Army myArmy) => myArmy.GetFrontUnit() == this;

        public virtual void ReceiveDamage(int damage)
        {
            int actual = damage - Defense;
            if (actual < 0) actual = 0;
            health -= actual;
        }

        public virtual void ReceiveHeal(int amount)
        {
            health += amount;
            if (health > maxHealth) health = maxHealth;
        }

        public virtual void RestoreHp(int hp) => health = Math.Min(hp, maxHealth);
    }
}