namespace StrategyGame.External
{
    /// <summary>
    /// Пример класса "Стена" без методов атаки/защиты, не наследуется от Unit.
    /// </summary>
    public class Wall
    {
        public int Health { get; private set; }

        public Wall(int initialHealth)
        {
            Health = initialHealth;
        }

        public void TakeDamage(int dmg)
        {
            Health -= dmg;
        }

        // Можем добавить любую другую логику, связанную со стеной,
        // но главное, что у неё нет attack/defense/cost/range и т.п.
    }
}
