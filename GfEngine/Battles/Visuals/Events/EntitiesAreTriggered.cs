namespace GfEngine.Battles.Visuals;

public record EntitiesAreTriggered : BattleVED
{
    public required List<string> TriggeredEntityIDs;
}