using GfEngine.Battles.Entities;
using GfEngine.Inputs;

namespace GfEngine.Battles.Systems
{
    public interface IBattleAgent
    {
        void EnqueueCommands(BattleInputContext context);
    }
}