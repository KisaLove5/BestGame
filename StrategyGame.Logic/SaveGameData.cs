using System.Collections.Generic;

namespace StrategyGame.Serialization
{
    public class SaveGameData
    {
        public List<UnitData> Army1 { get; set; } = new();
        public List<UnitData> Army2 { get; set; } = new();
    }

    public class UnitData
    {
        public string Type { get; set; } = string.Empty; // "Archer", "Mage" …
        public int Hp { get; set; }
    }
}
