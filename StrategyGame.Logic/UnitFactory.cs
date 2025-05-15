using StrategyGame.External;
using StrategyGame.Interfaces;
using StrategyGame.Strategies;
using StrategyGame.Adapters;
using StrategyGame.Proxies;

namespace StrategyGame
{
    public class UnitFactory : IUnitFactory
    {
        public Unit CreateUnit(string type)
        {
            Unit unit = null;

            switch (type.ToLower())
            {
                case "swordsman":
                    var sw = new Swordsman();
                    sw.SetAttackStrategy(new MeleeAttackStrategy());
                    unit = sw;
                    break;
                case "spearman":
                    var sp = new Spearman();
                    sp.SetAttackStrategy(new MultiTargetAttackStrategy());
                    unit = sp;
                    break;
                case "archer":
                    var ar = new Archer();
                    ar.SetAttackStrategy(new RangedAttackStrategy());
                    unit = ar;
                    break;
                case "mage":
                    var mg = new Mage();
                    mg.SetAttackStrategy(new MeleeAttackStrategy());
                    unit = mg;
                    break;
                case "healer":
                    var hl = new Healer();
                    hl.SetAttackStrategy(new MeleeAttackStrategy());
                    unit = hl;
                    break;
                case "wall":
                    unit = new WallAdapter(new Wall(25));
                    break;
                case "weaponbearer":
                    unit = new WeaponBearer();
                    break;
                default:
                    return null;
            }

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
