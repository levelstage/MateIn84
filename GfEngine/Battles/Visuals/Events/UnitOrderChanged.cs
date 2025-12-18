namespace GfEngine.Battles.Visuals
{
    public record UnitOrderChanged : BattleVED
    {
        public required List<(string, double)> NewUnitOrder { get; set; }
    }
}