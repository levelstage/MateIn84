using GfEngine.Battles.Core;
using GfEngine.Battles.Managers;
using GfEngine.Core;
using GfEngine.Systems.Commands;
using GfEngine.Visuals.BattleEvents;
using GfEngine.Inputs;
using GfEngine.Visuals.Commands;

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
        public IBehavior MakeBehavior(BattleInputContext context)  // 완성된 context로 스킬을 발동시키는 함수.
        {
            if(context.Caster == null) throw new InvalidDataException("Skill is activated with null caster.");
            context.Caster.CurrentAG = (double) Length / context.Caster.Speed;  // 턴 밀려나는 로직
            // 스킬마다 다른 처리 로직. 아무튼 IBehavior만 돌려주면 뭘 하든 상관 x.
            return OnCast(context);
        }
        public void ShowPreview(BattleInputContext context, Action onComplete)  // 스킬 미리보기 제공.
        {
            Sequence temp = new Sequence();
            if(context.Caster == null) throw new Exception("Invalid skill preview: No Caster");
            // 우선 공통 연출(다음 행동 순서 보여주기)을 시퀀스에 넣는다.
            temp.Enqueue(new VisualCommand(new UnitOrderChanged()
            {
                NewUnitOrder = TurnManager.Instance.PredictNextOrder(context.Caster, Length),
                IsPreview = true
            }));
            // 그 다음 스킬마다 다른 Preview 연출을 시퀀스에 넣는다.
            temp.Enqueue(OnPreview(context));
            // Preview를 띄우고, 다 띄우면 SkillInputCommand에서 받아온 콜백(유저에게 컨펌 받기)을 실행한다.
            temp.Execute(onComplete);
        }
        protected abstract IBehavior OnCast(BattleInputContext context);
        protected abstract IBehavior OnPreview(BattleInputContext context);
        public abstract SkillArgument RequiredArgument(BattleInputContext context);  // context에서 스킬을 발동하기에 부족한 argument를 return.
        public abstract SkillDomain GetDomain(BattleInputContext context);  // 플레이어용. 입력에 필요한 정의역만 받음.
        public abstract Skill Clone(); // 데이터베이스상의 스킬을 실제 유닛은 복사해서 갖게 됨.
    }
}