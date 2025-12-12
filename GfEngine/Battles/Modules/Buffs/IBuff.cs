using GfEngine.Battles.Units;

namespace GfEngine.Battles.Modules.Buffs
{
    public interface IBuff
    {
        string ID { get; }
        string Name { get; }
        string Description { get; }
        
        // 지속시간 (턴)
        int Duration { get; set; }
        // 중첩 (Stack)
        int Stack { get; set; }
        // 만료 여부 확인
        bool IsExpired { get; }
        
        // 버프의 속성 4가지
        bool IsRemovable { get; }
        bool IsBuff { get; }
        bool IsDebuff{ get; }
        bool IsVisible { get; }

        // [중요] 원본을 복제해서 유닛에게 줌
        IBuff Clone();

        // 생명주기
        void OnApply(Unit target);
        void OnRemove(Unit target);
        
        // 턴 감소 로직 (Unit.EndTurn 등에서 호출)
        void TickDuration(Unit target);
    }
}