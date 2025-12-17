using GfEngine.Inputs;
using GfEngine.Battles.Commands;
using GfEngine.Core;
using GfEngine.Battles.Entities;

namespace GfEngine.Battles.Systems
{
    public class PlayerBattleAgent : IBattleAgent
    {
        public IInputAdapter InputAdapter { get; private set; }
        public PlayerBattleAgent(IInputAdapter inputAdapter)
        {
            InputAdapter = inputAdapter;
        }
        public IBehavior MakeBehavior(Unit currentUnit)
        {
            Sequence sequence = new Sequence();
            sequence.Enqueue(new SkillInputCommand(new BattleInputContext(currentUnit), InputAdapter));
            return sequence;
        }
    }   
}