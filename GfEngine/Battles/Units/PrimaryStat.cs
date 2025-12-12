namespace GfEngine.Battles.Units
{
    public struct PrimaryStat
    {
        // === [1. 전사 계열 (Body)] ===
        public int Vit; // 생명력 (Vitality) -> HP, 물방
        public int End; // 지구력 (Endurance) -> 강인도, 물방, 무게 (성장불가)
        public int Str; // 근력 (Strength) -> 물공, 제압력, 무게

        // === [2. 척후 계열 (Technique)] ===
        public int Agi; // 민첩 (Agility) -> AG(Action Gauge)가 차는 속도
        public int Dex; // 솜씨 (Dexterity) -> 치명타 피해량
        public int Luk; // 행운 (Luck) -> 브레이크 저항, 드랍율 (성장불가)

        // === [3. 법사 계열 (Mind)] ===
        public int Spr; // 정신력 (Spirit) -> 강인도, 마방, 상태저항
        public int Mag; // 마력 (Magic) -> 마공, 마방, MP
        public int Int; // 지능 (Intelligence) -> MP, 경험치 획득량 (성장불가)

        // [편의 기능] 스탯끼리 더하기 (+) 연산자 오버로딩
        // 예: BaseStat + ItemStat 을 코드로 쉽게 쓰기 위함
        public static PrimaryStat operator +(PrimaryStat a, PrimaryStat b)
        {
            return new PrimaryStat 
            {
                Vit = a.Vit + b.Vit,
                End = a.End + b.End,
                Str = a.Str + b.Str,
                Agi = a.Agi + b.Agi,
                Dex = a.Dex + b.Dex,
                Luk = a.Luk + b.Luk,
                Spr = a.Spr + b.Spr,
                Mag = a.Mag + b.Mag,
                Int = a.Int + b.Int
            };
        }
    }
}