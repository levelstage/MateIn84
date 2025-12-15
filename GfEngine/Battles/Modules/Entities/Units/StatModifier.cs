namespace GfEngine.Battles.Entities
{
    // 어떤 스탯을 건드릴 거니?
    public enum StatType
    {
        // Primary
        Vit, End, Str, Agi, Dex, Luk, Spr, Mag, Int,
        // Derived
        MaxHP, MaxMP, AtkPhy, DefPhy, AtkMag, DefMag, MaxPoise, CritDmg
    }

    // 어떻게 건드릴 거니?
    public enum StatModType
    {
        Flat,       // 고정 수치 합산 (+10)
        PercentAdd, // 퍼센트 합산 (+10%) - 기본값 기준
        PercentMult // 퍼센트 곱연산 (*1.1) - 최종값 기준
    }

    public class StatModifier
    {
        public string SourceID; // 누가 붙였냐? (디버깅용: "buff_poison" 등)
        public StatType TargetStat;
        public StatModType Type;
        public float Value; // 값 (10.0f 등)

        public StatModifier(string sourceID, StatType target, StatModType type, float value)
        {
            SourceID = sourceID;
            TargetStat = target;
            Type = type;
            Value = value;
        }
    }
}