using GfEngine.Battles.Augments;
using GfEngine.Inputs;

namespace GfEngine.Battles.Systems
{
    public class InputCommand : ICommand
    {
        private readonly BattleInputContext _context;
        private readonly IInputAdapter _inputAdapter;
        public InputCommand(BattleInputContext context, IInputAdapter adapter)
        {
            _context = context;
            _inputAdapter = adapter;
        }
        public void Execute(Action onComplete)
        {
            if(_context.Caster == null)
            {
                throw new InvalidOperationException("InputCommand executed with null caster.");
            }
            if(_context.IsCancelled)
            {
                _context.Reset(_context.Caster);
            }
            // 아래 null에는 원래 유닛 리스트를 정렬해서 넣어줘야 한다. 아직 미구현.
            if(_context.SelectedSkillIndex == -1)
            {
                TurnManager.Instance.Queue.Enqueue(new InputCommand(_context, _inputAdapter));
                _inputAdapter.GetSkillIndex(_context, null, onComplete);
                return;
            }
            SkillArgument arg = _context.Caster.Skills[_context.SelectedSkillIndex].RequiredArgument(_context);
            if(arg == SkillArgument.None)
            {
                _context.Caster.Skills[_context.SelectedSkillIndex].EnqueueCommands(_context);
                onComplete();
            }
            else
            {
                if(arg == SkillArgument.XYs)
                {
                    TurnManager.Instance.Queue.Enqueue(new InputCommand(_context, _inputAdapter));
                    _inputAdapter.GetXYs(_context, null, onComplete);
                }
                else if(arg == SkillArgument.YesNo)
                {
                    TurnManager.Instance.Queue.Enqueue(new InputCommand(_context, _inputAdapter));
                    _inputAdapter.GetYesNo(_context, null, onComplete);
                }
            }
        }
    }
}