using GfEngine.Battles.Modules.Entities.Units;

namespace GfEngine.Battles.Modules.Augments.Buffs
{
    public abstract class Buff : Augment
    {   
        // 지속시간 (턴)
        public int Duration { get; set; }
        // 중첩 (Stack)
        public int Stack { get; set; }
        // 만료 여부 확인
        public bool IsExpired { get; }
        
        // 버프의 속성 4가지
        public bool IsRemovable { get; }
        public bool IsBuff { get; }
        public bool IsDebuff{ get; }
        public bool IsVisible { get; }

        // [중요] 원본을 복제해서 유닛에게 줌
        public abstract Buff Clone();

        // 생명주기
        public abstract void OnApply(Unit target);
        public abstract void OnRemove(Unit target);
        
        // 턴 감소 로직 (Unit.EndTurn 등에서 호출)
        public abstract void TickDuration(Unit target);
    }
}