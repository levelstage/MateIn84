using GfEngine.Battles.Augments;
using GfEngine.Inputs;
using GfEngine.Core;

namespace GfEngine.Battles.Commands
{
    public class SkillInputCommand : ICommand
    {
        private readonly BattleInputContext _originalContext;
        private readonly IInputAdapter _inputAdapter;
        private BattleInputContext _context;
        private Action? _onComplete;
        public SkillInputCommand(BattleInputContext originalContext, IInputAdapter adapter)
        {
            _originalContext = originalContext;
            _context = new BattleInputContext(_originalContext);
            _inputAdapter = adapter;
        }

        public void GetNextInput()
        {
            if(_context.IsCancelled)
            {
                _context = new BattleInputContext(_originalContext);
            }
            if(_context.Caster == null)
            {
                throw new InvalidOperationException("SkillInputCommand executed with null caster.");
            }
            if(_context.SelectedSkillID == "NONE")
            {
                // 밑에 있는 애한테, 현재 이 유닛의 스킬들의 목록, 쿨타임 등을 넘겨줘야 함.
                _inputAdapter.GetSkillIndex(_context, GetNextInput);
                return;
            }
            Skill skill = _context.Caster.Skills[_context.SelectedSkillID];
            SkillArgument arg = skill.RequiredArgument(_context);
            if(arg == SkillArgument.None)
            {
                if(Sequence.Running == null) throw new Exception("A skill must be casted in a sequence.");
                // 이 입력 커맨드가 있는 Behavior가 종료된 다음 바로 스킬이 나가야 하기 때문에 Interrupt.
                Sequence.Running.Interrupt(skill.MakeBehavior(_context));
                _onComplete?.Invoke();
            }
            else
            {
                SkillDomain domain = skill.GetDomain(_context);
                if(arg == SkillArgument.XYs)
                {
                    _inputAdapter.GetXYs(_context, domain, GetNextInput);
                }
                else if(arg == SkillArgument.YesNo)
                {
                    skill.ShowPreview(_context, () =>
                    {
                        _inputAdapter.GetYesNo(_context, GetNextInput);
                    });
                }
            }
        }

        public void Execute(Action onComplete)
        {
            _onComplete = onComplete;
            GetNextInput();
        }
    }
}