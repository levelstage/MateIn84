using GfEngine.Visuals.Events;
namespace GfEngine.Visuals.Handlers
{    
    public interface IVisualEventHandler {
        void Handle(VisualEventData @event, Action onComplete);
    }
}