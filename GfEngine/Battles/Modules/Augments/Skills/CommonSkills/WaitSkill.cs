using GfEngine.Inputs;
using GfEngine.Battles.Core;

namespace GfEngine.Battles.Augments.CommonSkills
{
    public class WaitSkill : Skill
    {
        public WaitSkill() : base() { Length = TurnLength.Half; }
        public WaitSkill(WaitSkill o) : base(o) {  
            Length = o.Length;
        }
        // 1. 필요한 인자: 없음 (바로 실행 단계로!)
        public override SkillArgument RequiredArgument(BattleInputContext context)
        {
            return SkillArgument.None;
        }

        // 2. 도메인: 인자가 없으니 도메인도 사실상 의미 없음 (빈 객체 리턴)
        public override SkillDomain GetDomain(BattleInputContext context)
        {
            return new SkillDomain { XYCounts = 0 }; // 타겟팅 안 함
        }

        // 3. 미리보기: "내 턴이 언제 다시 돌아오는지"만 보여주면 됨. 다른건 변화 x
        protected override void OnPreview(BattleInputContext context)
        {
            // 하는 일 아예 없음. (공통 미리보기(행동순서)만 있으면 충분.)
        }

        // 4. 실행
        protected override void OnCast(BattleInputContext context)
        {
            // 역시 아무것도 하지 않는다. (공통 행동(턴 넘어가기)만 함.)
        }

        public override Skill Clone()
        {
            return new WaitSkill(this);
        }
    }
}