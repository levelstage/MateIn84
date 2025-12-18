using GfEngine.Battles.Entities;
using GfEngine.Core.Commands;
using GfEngine.Inputs;

namespace GfEngine.Battles.Systems
{
    public interface IBattleAgent
    {
        IBehavior MakeBehavior(Unit currentUnit);
    }
}