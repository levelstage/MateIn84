using GfEngine.Core;
using GfEngine.Systems;
using GfEngine.Visuals.Events;

namespace GfEngine.Visuals.Commands
{
    // 엔진 내의 단발성 처리를 수행하는 커맨드
    public class VisualCommand : ICommand
    {
        private VisualEventData _ved;

        public VisualCommand(VisualEventData ved)
        {
            _ved = ved;
        }

        public void Execute(Action onComplete)
        {
            if(SessionManager.Instance.Visualizer == null) throw new Exception("Visualizer is not injected to SessionManager.");
            SessionManager.Instance.Visualizer.Handle(_ved, onComplete);
        }
    }
}