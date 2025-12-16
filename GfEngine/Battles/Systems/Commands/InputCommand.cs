using GfEngine.Battles.Augments;
using GfEngine.Inputs;
using GfEngine.Core;

namespace GfEngine.Battles.Commands
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
            if(_context.SelectedSkillIndex == -1)
            {
                CommandQueue.Instance.Enqueue(new InputCommand(_context, _inputAdapter));
                // 밑에 있는 애한테, 현재 이 유닛의 스킬들의 목록, 쿨타임 등을 넘겨줘야 함.
                _inputAdapter.GetSkillIndex(_context, onComplete);
                return;
            }
            Skill skill = _context.Caster.Skills[_context.SelectedSkillIndex];
            SkillArgument arg = skill.RequiredArgument(_context);
            if(arg == SkillArgument.None)
            {
                skill.Cast(_context);
                onComplete();
            }
            else
            {
                SkillDomain domain = skill.GetDomain(_context);
                if(arg == SkillArgument.XYs)
                {
                    CommandQueue.Instance.Enqueue(new InputCommand(_context, _inputAdapter));
                    _inputAdapter.GetXYs(_context, domain, onComplete);
                }
                else if(arg == SkillArgument.YesNo)
                {
                    skill.ShowPreview(_context, () =>
                    {
                        CommandQueue.Instance.Enqueue(new InputCommand(_context, _inputAdapter));
                        _inputAdapter.GetYesNo(_context, onComplete);
                    });
                }
            }
        }
    }
}