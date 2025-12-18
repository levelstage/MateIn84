using GfEngine.Core.Commands;

namespace GfEngine.Systems.Commands
{
    // 엔진 내의 단발성 처리를 수행하는 커맨드. 엔진 안에서만 쓰므로 역시 internal
    internal class InternalCommand : ICommand
    {
        private Action _action;
        private bool _skipCallBack = false;

        public InternalCommand(Action action, bool skipCallBack = false)
        {
            _action = action;
            _skipCallBack = skipCallBack;
        }

        public void Execute(Action onComplete)
        {
            // 로직 즉시 실행
            _action?.Invoke();
            
            // 로직은 대기 시간이 없으므로 즉시 완료 보고
            if(!_skipCallBack) onComplete?.Invoke();
        }
    }
}