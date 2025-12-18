using GfEngine.Battles.Augments;

namespace GfEngine.Inputs
{
    public interface IInputAdapter
    {
        void GetSkillIndex(BattleInputContext context, SkillDomain domain, Action onComplete);
        void GetXYs(BattleInputContext context, SkillDomain domain, Action onComplete);
        void GetYesNo(BattleInputContext context, Action onComplete);
    }
}