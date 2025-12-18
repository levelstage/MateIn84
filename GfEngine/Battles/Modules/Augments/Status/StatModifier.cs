using GfEngine.Battles.Core;
namespace GfEngine.Battles.Augments
{
    public class StatModifier : Augment 
    {
        public string? SourceID { get; set; } // 누가 붙였는지 추적을 위한 ID
        public StatType TargetStat;
        public StatModType Type;
        public float Value; // 값 (10.0f 등)
    }
}