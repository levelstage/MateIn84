using GfEngine.Battles.Systems;
using GfEngine.Inputs;

namespace GfEngine.Battles.Augments
{

    public abstract class Skill : Augment
    {
        public abstract ICommand EnqueueCommands(BattleInputContext context);
        public abstract SkillArgument RequiredArgument(BattleInputContext context);
    }
}