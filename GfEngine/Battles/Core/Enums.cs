namespace GfEngine.Battles.Core
{
    public enum TurnLength
    {
        Quarter = 2500, // 쿼터 턴 (대기, 무기 스왑)
        Half = 5000, // 하프 턴 (이동, 방어, 유틸기)
        Full = 10000 // 풀 턴 (공격, 스킬)
    }
    public enum StatModType
    {
        Flat,       // 고정 수치 합산 (+10)
        PercentAdd, // 퍼센트 합산 (+10%) - 기본값 기준
        PercentMult // 퍼센트 곱연산 (*1.1) - 최종값 기준
    }
    public enum StatType
    {
        // Primary
        Vit, End, Str, Agi, Dex, Luk, Spr, Mag, Int,
        // Derived
        MaxHP, MaxMP, AtkPhy, DefPhy, AtkMag, DefMag, MaxPoise, CritDmg, Speed
    }
    public enum SkillArgument
    {
        None,
        XYs,
        YesNo
    }
}