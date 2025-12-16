using GfEngine.Battles.Core;
using GfEngine.Battles.Managers;
using GfEngine.Core;
using GfEngine.Systems.Commands;
using GfEngine.Visuals.BattleEvents;
using GfEngine.Inputs;

namespace GfEngine.Battles.Augments
{

    public abstract class Skill : Augment
    {
        public TurnLength Length { get; set; } = TurnLength.Full; // 이 스킬이 잡아먹는 턴 길이.
        public Skill() : base() {}
        public Skill(Skill o) : base(o)
        {
            Length = o.Length;
        }
        public void Cast(BattleInputContext context)  // 완성된 context로 스킬을 발동시키는 함수.
        {
            if(context.Caster == null) throw new InvalidDataException("Skill is activated with null caster.");
            context.Caster.CurrentAG = (double) Length / context.Caster.Speed;
            OnCast(context);
            CommandQueue.Instance.Run();
        }
        public void ShowPreview(BattleInputContext context, Action onComplete)  // 스킬 미리보기 제공.
        {
            if(context.Caster == null) throw new Exception("Invalid skill preview: No Caster");
            Enqueue(new VisualCommand(new UnitOrderChanged()
            {
                NewUnitOrder = TurnManager.Instance.PredictNextOrder(context.Caster, Length),
                IsPreview = true
            }));
            OnPreview(context);
            Enqueue(new InternalCommand(()=>{ onComplete?.Invoke(); }, skipCallBack: true));
            CommandQueue.Instance.Run(); // 장전된 프리뷰용 커맨드들에 불을 붙인다.
        }
        protected void Enqueue(ICommand command)
        {
            CommandQueue.Instance.Enqueue(command);
        }
        protected abstract void OnCast(BattleInputContext context);
        protected abstract void OnPreview(BattleInputContext context);
        public abstract SkillArgument RequiredArgument(BattleInputContext context);  // context에서 스킬을 발동하기에 부족한 argument를 return.
        public abstract SkillDomain GetDomain(BattleInputContext context);  // 플레이어용. 입력에 필요한 정의역만 받음.
        public abstract Skill Clone(); // 데이터베이스상의 스킬을 실제 유닛은 복사해서 갖게 됨.
    }
}