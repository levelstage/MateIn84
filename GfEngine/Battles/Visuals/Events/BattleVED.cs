using GfEngine.Visuals.Events;

namespace GfEngine.Battles.Visuals
{
    public record BattleVED : VisualEventData
    {
        public bool IsPreview { get; set; } = false;
    }
}