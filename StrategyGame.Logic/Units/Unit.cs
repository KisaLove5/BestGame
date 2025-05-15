using System.Text;
using StrategyGame.Interfaces;

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
        protected IAttackStrategy attackStrategy;

        private static int idCounter = 0;
        private readonly int unitId = idCounter++;

        public virtual int UnitId => unitId;

        public virtual int Health => health;
        public virtual int MaxHealth => maxHealth;
        public virtual int AttackValue => attack;
        public virtual int Defense => defense;
        public virtual int Cost => cost;
        public virtual int Range => range;

        private string displayName;

        private static readonly Random _rnd = new();
        private const double ChanceDecay = 0.70;  // после успеха ×0.7
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

            // ПРОВАЛ
            sb.AppendLine($"{unitTag} промахивается имея шанс {_currentChance}! Шанс сброшен.");
            _currentChance = ChanceReset;
            return false;
        }

        public virtual string DisplayName
        {
            get => displayName ?? this.GetType().Name;
            set => displayName = value;
        }

        public void SetAttackStrategy(IAttackStrategy strategy)
        {
            attackStrategy = strategy;
        }

        public virtual void Attack(Army myArmy, Army enemyArmy)
        {
            attackStrategy?.ExecuteAttack(this, myArmy, enemyArmy);
        }

        public virtual void ReceiveDamage(int damage)
        {
            int actual = damage - defense;
            if (actual < 0) actual = 0;
            health -= actual;
        }

        public virtual void DoPersonalAction(Army myArmy, Army enemyArmy, StringBuilder sb)
        {
        }

        protected bool IsFront(Army myArmy)
        {
            return myArmy.GetFrontUnit() == this;
        }

        public void RestoreHp(int hp) => health = Math.Min(hp, maxHealth);
    }
}
