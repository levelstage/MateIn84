using GfEngine.Visuals.Events;
namespace GfEngine.Visuals.Handlers
{    
    // 편의를 위한 제네릭 부모 클래스
    public abstract class VisualEventHandler<T> : IVisualEventHandler where T : VisualEventData {
        public void Handle(VisualEventData @event, Action onComplete) {
            if (@event is T t) Handle(t, onComplete);
        }
        protected abstract void Handle(T @event, Action onComplete);
    }
}