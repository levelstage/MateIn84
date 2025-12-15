using System;
using GfEngine.Battles.Entities;
using GfEngine.Battles.Systems; // Command가 있는 곳

namespace GfEngine.Inputs
{
    public interface IInputAdapter
    {
        void GetSkillIndex(BattleInputContext context, List<Unit> unitsOrderedByAG, Action onComplete);
        void GetXYs(BattleInputContext context, List<Unit> unitsOrderedByAG, Action onComplete);
        void GetYesNo(BattleInputContext context, List<Unit> unitsOrderedByAG, Action onComplete);
    }
}