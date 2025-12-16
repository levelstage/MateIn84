using GfEngine.Inputs;
using GfEngine.Battles.Commands;
using GfEngine.Core;

namespace GfEngine.Battles.Systems
{
    public class PlayerBattleAgent : IBattleAgent
    {
        public IInputAdapter InputAdapter { get; private set; }
        public PlayerBattleAgent(IInputAdapter inputAdapter)
        {
            InputAdapter = inputAdapter;
        }
        public void EnqueueCommands(BattleInputContext context)
        {
            CommandQueue.Instance.Enqueue(new InputCommand(context, InputAdapter));
        }
    }   
}