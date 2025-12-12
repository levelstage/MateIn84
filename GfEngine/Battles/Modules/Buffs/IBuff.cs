using GfEngine.Battles.Units;

namespace GfEngine.Battles.Modules.Buffs
{
    public interface IBuff
    {
        // 기본 정보
        string ID { get; }
        string Name { get; }
        string Description { get; }
        string IconPath { get; }
        
        // 상태 (인스턴스마다 다름)
        int Duration { get; set; } // 남은 턴
        int Stack { get; set; }    // 중첩 수
        bool IsExpired { get; }    // "나 끝났어? 지워줘."

        // === [1. 복제 (필수)] ===
        // 리플렉션으로 원본을 가져오면, 유닛에게 줄 때는 '복사본'을 줘야 함
        IBuff Clone(); 

        // === [2. 생명주기 훅 (Logic)] ===
        void OnApply(Unit target);      // 처음 걸렸을 때
        void OnRemove(Unit target);     // 지워질 때 (스탯 원복 등)
    }
}