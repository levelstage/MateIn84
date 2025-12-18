using GfEngine.Battles.Augments;
using GfEngine.Battles.Visuals;
using GfEngine.Core.Commands;
using GfEngine.Core.Events;
using GfEngine.Visuals.Commands;

namespace GfEngine.Battles.Managers
{
    public class BattleEventManager
    {
        private static BattleEventManager? _instance;
        public static BattleEventManager Instance => _instance??= new();
        private readonly Dictionary<string, List<IEventListener>> _eventMap = new();
        public void Subscribe(string @event, IEventListener subscriber)
        {
            if(!_eventMap.TryGetValue(@event, out var listeners)) {
                listeners = new();
                _eventMap[@event] = listeners;
            }
            listeners.Add(subscriber);
        }
        public void Unsubscribe(string @event, IEventListener subscriber)
        {
            if(_eventMap.TryGetValue(@event, out var listeners)) {
                listeners.Remove(subscriber);
            }
        }
        public Sequence? React(string @event, GameEventData data)
        {
            List<string> triggeredEntityIDs = new();
            Sequence eventPhase = new();
            if(_eventMap.TryGetValue(@event, out var listeners))
            {
                foreach(var listener in listeners.OrderByDescending(l => l.Priority).ToArray())
                {
                    var reaction = listener.React(data);
                    if(reaction != null)
                    {
                        eventPhase.Enqueue(reaction);
                        if(reaction is Trigger trigger)
                        {
                            triggeredEntityIDs.Add(trigger.OwnerID);
                        }
                    }
                }
            }
            if(triggeredEntityIDs.Count > 0)
            {
                // 트리거가 발동한 엔티티가 하나라도 있다면, 비주얼라이저 측에서 무언가 일을 할 것이다.
                eventPhase.Interrupt(new VisualCommand(new EntitiesAreTriggered() {
                     TriggeredEntityIDs = triggeredEntityIDs
                } ));
            }
            return eventPhase.IsEmpty ? null : eventPhase;
        }
    }
}