using GfEngine.Systems.Commands;

namespace GfEngine.Core
{
    // Sequence는 Behavior의 인터페이스를 사용하지만 Behavior는 아님. 대신 BehaviorQueue에는 들어갈 수 있다.
    public class Sequence : IBehavior
    {
        // 이벤트 등이 발생할 때 어떤 시퀀스에 끼워야할지 알려주기 위한 스태틱.
        internal static Sequence? Running { get; private set; }
        // LinkedList를 활용해 Enqueue, Interrupt 등을 구현해주는 행동 큐.
        private BehaviorQueue _queue;
        public Sequence()
        {
            _queue = new BehaviorQueue();
        }
        public void Execute(Action onComplete)
        {
            Sequence? prev = Running;
            // 가동중인 시퀀스를 자신으로 변경함.
            Running = this;
            // 예약된 행동 큐의 맨 끝에 콜백을 집어넣고 돌린다. 큐 내부 콜백(큐 밀기)는 필요 없으므로 스킵.
            Enqueue(new InternalCommand(() => { Running = prev; onComplete(); }, skipCallBack: true));
            // nest된 시퀀스가 종료되면 다시 가동중 시퀀스를 자신으로 변경.
            _queue.Run();
        }
        // 커맨드 Enqueue
        public void Enqueue(ICommand command)
        {
            _queue.Enqueue(new Behavior(command));
        }
        // 일반적으로 사용하게 되는 Enqueue
        public void Enqueue(IBehavior behavior)
        {
            _queue.Enqueue(behavior);
        }
        // 행동 끼어들기(이벤트 등 처리)
        public void Interrupt(IBehavior behavior)
        {
            _queue.Interrupt(behavior);
        }

    }
}