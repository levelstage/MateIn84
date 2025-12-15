using GfEngine.Battles.Entities;
using GfEngine.Inputs;

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
            TurnManager.Instance.Queue.Enqueue(new InputCommand(context, InputAdapter));
        }
    }   
}