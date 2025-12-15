using GfEngine.Visuals.Events;

namespace GfEngine.Visuals
{    
    public interface IVisualizer
    {
        void Handle(VisualEventData data, Action onComplete);
    }
}