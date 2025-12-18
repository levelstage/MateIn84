using GfEngine.Core.Commands;
using GfEngine.Core.Events;

namespace GfEngine.Battles.Augments;

public abstract class Trigger : Augment, IEventListener
{
    public int Priority { get; protected set; }
    public abstract IBehavior React(GameEventData data);
}