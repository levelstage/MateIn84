using System;

namespace GfEngine.Battles.Systems
{
    // 로직(데미지 계산 등)을 수행하는 커맨드
    public class ActionCommand : ICommand
    {
        private Action _action;

        public ActionCommand(Action action)
        {
            _action = action;
        }

        public void Execute(Action onComplete)
        {
            // 로직 즉시 실행
            _action?.Invoke();
            
            // 로직은 대기 시간이 없으므로 즉시 완료 보고
            onComplete?.Invoke();
        }
    }
}