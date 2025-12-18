namespace GfEngine.Core.Commands
{
    // Sequence는 Behavior의 인터페이스를 사용하지만 Behavior는 아님. 대신 다른 Sequence에 들어갈 수 있다.
    public class Sequence : IBehavior
    {
        // 이벤트 등이 발생할 때 어떤 시퀀스에 끼워야할지 알려주기 위한 스태틱.
        internal static Sequence? Running { get; private set; }
        // LinkedList를 활용해 Enqueue, Interrupt 등을 구현해주는 행동 큐.
        private LinkedList<IBehavior> _list;
        public bool IsEmpty => _list.Count == 0;
        private Action? _onComplete;
        private Sequence? _prev;
        public Sequence()
        {
            _list = new();
        }
        public void Execute(Action onComplete)  // 실행 (IBehavior 인터페이스)
        {
            // 이전에 가동되던 시퀀스를 담아둠. 시퀀스가 끝나면 가동 상태를 변경한다.
            _prev = Running;
            // 가동중인 시퀀스를 자신으로 변경함.
            Running = this;
            // onComplete를 담아두고, 큐가 비워지면 실행!
            _onComplete = onComplete;
            // 시퀀스 기동. 큐 밀기!
            TryRunNext();
        }
        // 커맨드 Enqueue (IBehavior 인터페이스)
        public void Enqueue(ICommand command)
        {
            _list.AddLast(new Behavior(command));
        }
        // 일반적으로 사용하게 되는 Enqueue
        public void Enqueue(IBehavior behavior)
        {
            _list.AddLast(behavior);
        }
        // 커맨드 끼어들기 (인터페이스에는 없음.)
        public void Interrupt(ICommand behavior)
        {
            _list.AddFirst(new Behavior(behavior));
        }
        // Behavior 끼어들기(이벤트 등 처리)
        public void Interrupt(IBehavior behavior)
        {
            _list.AddFirst(behavior);
        }

        // 도미노 밀기 시작
        private void TryRunNext()
        {
            // 리스트에 아무것도 없다면, 시퀀스 실행 상태를 원복하고 
            if (_list.First == null)
            {
                Running = _prev;
                _onComplete?.Invoke();
                return;
            }
            // 실행 시작
            IBehavior behavior = _list.First.Value;
            _list.RemoveFirst();

            // Behavior 실행! (끝나면 콜백 호출하며 다음 도미노 넘어뜨리기)
            behavior?.Execute(TryRunNext);
        }
    }
}