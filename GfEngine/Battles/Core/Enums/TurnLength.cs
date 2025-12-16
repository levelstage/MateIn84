namespace GfEngine.Battles.Core
{
    public enum TurnLength
    {
        Quarter = 2500, // 쿼터 턴 (대기, 무기 스왑)
        Half = 5000, // 하프 턴 (이동, 방어, 유틸기)
        Full = 10000 // 풀 턴 (공격, 스킬)
    }
}