using StrategyGame.External;
using StrategyGame.Interfaces;
using StrategyGame.Adapters;
using StrategyGame.Proxies;

namespace StrategyGame
{
    /// <summary>
    /// Фабрика создания юнитов. Настраивает базовые декораторы/прокси,
    /// но не управляет логикой атаки (она теперь встроена в сами юниты).
    /// </summary>
    public class UnitFactory : IUnitFactory
    {
        public Unit CreateUnit(string type)
        {
            Unit unit = type.ToLower() switch
            {
                "swordsman" => new Swordsman(),
                "spearman" => new Spearman(),
                "archer" => new Archer(),
                "mage" => new Mage(),
                "healer" => new Healer(),
                "wall" => new WallAdapter(new Wall(25)),
                "weaponbearer" => new WeaponBearer(),
                _ => null
            };

            if (unit != null)
            {
                if (GameSettings.EnableDeathLogging)
                {
                    unit = new Proxies.DeathLoggingProxy(unit);
                }

                if (GameSettings.EnableAbilityLogging)
                {
                    unit = new Proxies.AbilityLoggingProxy(unit);
                }
            }

            return unit;
        }
    }
}