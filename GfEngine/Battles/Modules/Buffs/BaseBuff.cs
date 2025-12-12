using System;
using System.Collections.Generic;
using GfEngine.Battles.Systems;
using GfEngine.Battles.Units;

namespace GfEngine.Battles.Modules.Buffs
{
    public abstract class BaseBuff : IBuff
    {
        public abstract string ID { get; }
        public virtual string Name => "Unknown Buff";
        public virtual string Description => "";
        public virtual string IconPath => "icons/default_buff";

        public int Duration { get; set; } = 3; // 기본 3턴
        public int Stack { get; set; } = 1;
        public bool IsExpired => Duration <= 0;

        // Clone은 자식이 구현해야 함 (자신을 new 해야 하니까)
        public abstract IBuff Clone();

        // 나머지는 빈 깡통으로 둠 (필요한 것만 override)
        public virtual void OnApply(Unit target) { }

        // 내가 구독한 목록 기억
        private Dictionary<string, Action<BattleEventData>> _mySubscriptions 
            = new Dictionary<string, Action<BattleEventData>>();

        public virtual void OnRemove(Unit target) 
        {
            foreach (var kvp in _mySubscriptions)
            {
                target.Unsubscribe(kvp.Key, kvp.Value);
            }
            _mySubscriptions.Clear();
            
            // ... (스탯 제거 로직 등) ...
        }

        // [Helper] 자식용 등록 함수
        protected void RegisterEvent(Unit target, string eventName, Action<BattleEventData> callback)
        {
            target.Subscribe(eventName, callback);
            _mySubscriptions.Add(eventName, callback);
        }
    }
}