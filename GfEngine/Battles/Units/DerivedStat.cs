namespace GfEngine.Battles.Units
{
    public struct DerivedStat
    {
        // === [자원 (Resources)] ===
        public int MaxHP;
        public int MaxMP;
        public int MaxPoise;    // 강인도 (End + Spr)
        public int MaxWeight;   // 무게 한도 (Str + End)

        // === [공격 (Offense)] ===
        public int AtkPhy;      // 물리 공격력
        public int AtkMag;      // 마법 공격력
        public int Suppression; // 제압력 (강인도 깎는 힘)
        public int CritDmg;     // 치명타 피해량 (%)

        // === [방어 (Defense)] ===
        public int DefPhy;      // 물리 방어력
        public int DefMag;      // 마법 방어력
        public int CritResist;  // 브레이크 시 치명타 저항률 (%)
        
        // === [유틸 (Utility)] ===
        public int Speed; // 속도
        public int DropRateBonus; // 아이템 드랍 확률 보정
    }
}