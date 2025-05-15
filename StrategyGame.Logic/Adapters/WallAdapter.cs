using StrategyGame.External;
using StrategyGame.Interfaces;

namespace StrategyGame.Adapters
{
    /// <summary>
    /// Адаптер, позволяющий "Wall" выглядеть как "Unit".
    /// </summary>
    public class WallAdapter : Unit
    {
        private Wall _wall;

        private int _initialHealth;

        public WallAdapter(Wall wall)
        {
            _wall = wall;
            _initialHealth = wall.Health; // Сохраняем стартовое здоровье

            // Условно присвоим параметры Unit,
            // чтобы Army могла видеть: cost=??? range=??? 
            // Если стена бесплатна и не бьёт: 
            this.maxHealth = health;
            this.cost = 60;
            this.attack = 0;
            this.defense = 0;
            this.range = 0;
            this.DisplayName = "WallAdapter";
            // "health" будем перенаправлять к _wall
        }
        
        public override string BaseTypeName => "Wall";

        // Переопределим Health, чтобы читалось из Wall
        public override int Health
        {
            get { return _wall.Health; }
        }

        public override int MaxHealth
        {
            get { return _initialHealth; }
        }

        public override void Attack(Army myArmy, Army enemyArmy)
        {
            // Стена не атакует
        }

        public override void ReceiveDamage(int damage)
        {
            // Передаём урон во Wall
            // (defense=0, так что отняли как есть)
            _wall.TakeDamage(damage);
        }

        public override void RestoreHp(int hp)
        {
            typeof(Wall).GetProperty("Health")!
                        .SetValue(_wall, Math.Min(hp, _initialHealth));
        }
    }
}
