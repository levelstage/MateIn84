using System;
using System.Collections.Generic;
using GfEngine.Battles.Systems; // BattleEventData
using GfEngine.Battles.Units;   // StatModifier

namespace GfEngine.Battles.Modules.Buffs
{
    public abstract class BaseBuff : IBuff
    {
        public abstract string ID { get; }
        public virtual string Name => "Unknown Buff";
        public virtual string Description => "";

        public int Duration { get; set; } = 3; // 기본 3턴
        public int Stack { get; set; } = 1;
        public bool IsExpired => Duration <= 0;

        public virtual bool IsBuff => true;
        public virtual bool IsDebuff => false;
        public virtual bool IsRemovable => true;
        public virtual bool IsVisible => true;

        // Clone은 자식이 직접 구현 (new MyBuff())
        public abstract IBuff Clone();

        // === [자동 수거함 (Garbage Collection)] ===
        // 내가 만든 모디파이어와 이벤트 구독을 기억해둠
        private List<StatModifier> _myModifiers = new List<StatModifier>();
        private Dictionary<string, Action<BattleEventData>> _mySubscriptions 
            = new Dictionary<string, Action<BattleEventData>>();

        // === [생명주기] ===
        public virtual void OnApply(Unit target) 
        { 
            // 자식 클래스에서 override해서 로직 구현
        }

        public virtual void OnRemove(Unit target) 
        {
            // 1. 스탯 모디파이어 회수
            foreach (var mod in _myModifiers)
            {
                target.RemoveModifier(mod);
            }
            _myModifiers.Clear();

            // 2. 이벤트 구독 해지
            foreach (var kvp in _mySubscriptions)
            {
                target.Unsubscribe(kvp.Key, kvp.Value);
            }
            _mySubscriptions.Clear();

            // 3. 스탯 재계산 (변화가 있었으니)
            target.UpdateStats();
            
            Console.WriteLine($"[Buff] {Name} 효과 종료 및 정리 완료.");
        }

        public virtual void TickDuration(Unit target)
        {
            Duration--;
            if (IsExpired)
            {
                // 실제 제거는 Unit이나 BattleManager가 IsExpired를 보고 호출함
                // 여기서는 로그 정도만
                // Console.WriteLine($"{Name} 지속시간 끝남.");
            }
        }

        // === [Helper Methods (자식들이 쓸 도구)] ===
        
        // 1. 스탯 조작 등록
        protected void AddStatModifier(Unit target, StatType type, StatModType modType, float value)
        {
            var mod = new StatModifier(ID, type, modType, value);
            _myModifiers.Add(mod);     // 장부 기록
            target.AddModifier(mod);   // 실제 적용
        }

        // 2. 이벤트 구독 등록
        protected void RegisterEvent(Unit target, string eventName, Action<BattleEventData> callback)
        {
            target.Subscribe(eventName, callback); // 실제 구독
            _mySubscriptions.Add(eventName, callback); // 장부 기록
        }
    }
}