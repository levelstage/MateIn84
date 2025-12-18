using GfEngine.Battles.Core;
using GfEngine.Battles.Augments;
using GfEngine.Battles.Systems;
using GfEngine.Core;
using GfEngine.Core.Commands;
using GfEngine.Battles.Managers;
using GfEngine.Systems.Commands;
using System.Buffers;

namespace GfEngine.Battles.Entities
{

    public class Unit : Entity, IAGHolder
    {
        public int this[StatType type] => _status[type];
        // === [기본 정보] ===
        public IBattleAgent? Controller { get; set; }
        public int Level { get; set; } = 1;
        // 유닛의 4대 게이지. 체력, 마나, 강인도, 행동 게이지
        public int CurrentHP { get; private set; }
        public int CurrentMP { get; private set; }
        public int CurrentPoise { get; private set; }
        private int _currentAG;
        public int CurrentAG  // (IAGHolder 인터페이스)
        {
            get
            {
                return _currentAG;
            }
            set
            {
                _currentAG = value >= 0 ? value : 0;
            }
        }
        public bool IsDead => CurrentHP == 0;
        // 이 유닛에 붙어있는 Augment들
        public CodeContainer Inventory { get; set; }  // 아이템 박스. 이 유닛의 인벤토리.
        public CodeContainer Skills { get; set; }  // 스킬 박스. 이 유닛이 가진 모든 스킬.
        public CodeContainer Traits { get; set; }  // 특성 박스. 이 유닛이 가진 모든 특성.
        public CodeContainer Triggers { get; set; }  // 트리거 박스. 이 유닛이 가진 모든 트리거.
        private readonly Status _status;
        public int Speed => _status[StatType.Speed]; // (IAGHolder 인터페이스)

        public void OnTurnStart(Action onComplete) // (IAGHolder 인터페이스)
        {
            if(Controller == null) throw new Exception("Unit must process turn with Controller");
            TurnManager.Instance.CurrentUnit = this;
            Sequence mainPhase = new Sequence();
            var reaction = BattleEventManager.Instance.React(BattleEventName.ON_TURN_START, null);
            if(reaction != null)mainPhase.Enqueue(reaction);
            mainPhase.Enqueue(Controller.MakeBehavior(this));
            mainPhase.Execute(() => {OnTurnEnd(onComplete);});
        }

        private void OnTurnEnd(Action onComplete)
        {
            Sequence? endPhase = BattleEventManager.Instance.React(BattleEventName.ON_TURN_END, null);
            if(endPhase == null)
            {
                TurnManager.Instance.CurrentUnit = null;
                onComplete?.Invoke();
                return;
            }
            endPhase.Execute(() =>
            {
                TurnManager.Instance.CurrentUnit = null;
                onComplete?.Invoke();
            });
        }

        public Unit(Status status)
        {
            _status = status;
            Inventory = new();
            Skills = new();
            Traits = new();
            Triggers = new();
        }

        // AG 리셋
        public void ResetAG(TurnLength length)
        {
            CurrentAG = (int)length; 
        }

        // AG 초기화
        public void InitializeAG()
        {
            // 첫 턴은 하프 턴 기준으로 스타트.
            CurrentAG = (int)TurnLength.Half; 
        }

        public List<string> GetAvailableSkills()
        {
            return new();
        }

    }
}