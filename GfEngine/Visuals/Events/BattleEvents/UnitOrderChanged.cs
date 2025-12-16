using GfEngine.Battles.Entities;

namespace GfEngine.Visuals.BattleEvents
{
    public class UnitOrderChanged : BattleVED
    {
        public required List<(string, double)> NewUnitOrder { get; set; }
    }
}