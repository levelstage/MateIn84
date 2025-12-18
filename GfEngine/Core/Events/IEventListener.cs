using GfEngine.Core.Commands;

namespace GfEngine.Core.Events
{
    public interface IEventListener
    {
        int Priority { get; }
        IBehavior React(GameEventData data);
    }
}