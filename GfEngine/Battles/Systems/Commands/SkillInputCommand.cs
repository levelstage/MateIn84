using GfEngine.Battles.Augments;
using GfEngine.Inputs;
using GfEngine.Core.Commands;
using GfEngine.Battles.Managers;
using GfEngine.Battles.Core;
using System.Collections;

namespace GfEngine.Battles.Commands
{
    // 플레이어에게 어떤 스킬을 사용할지, 어떤 대상에게 사용할지 등을 입력받기 위한 커맨드.
    public class SkillInputCommand : ICommand
    {
        // 취소를 하면 어느 지점까지 돌아가는지를 결정하는 Context.
        private readonly BattleInputContext _originalContext;
        // 입력을 받기 위한 단자. 여기에 클라이언트의 입력 시스템을 꽂아서 활용함.
        private readonly IInputAdapter _inputAdapter;
        // 현재 입력중인 상태를 저장하는 Context.
        private BattleInputContext _context;
        // 커맨드의 콜백을 저장해놨다가 입력이 끝나면 호출한다.
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
                // 밑에 있는 애한테, 현재 이 유닛의 스킬들의 목록, 쿨타임 등을 넘겨줘야 함. (그건 Unit을 받아서 알아서 표시하고, 그냥 가능한 스킬 목록만 던져주자.)
                _inputAdapter.GetSkillIndex(_context, new SkillDomain() { ValidSkillIDs = _context.Caster.GetAvailableSkills() }, GetNextInput);
                return;
            }
            Skill skill = AugmentManager.Instance.GetSkill(_context.SelectedSkillID);
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