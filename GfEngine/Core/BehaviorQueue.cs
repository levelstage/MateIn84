namespace GfEngine.Core
{
    public class BehaviorQueue
    {
        private LinkedList<IBehavior> _list = new LinkedList<IBehavior>();

        // Behavior 추가 (도미노 세우기)
        public void Enqueue(IBehavior behavior)
        {
            _list.AddLast(behavior);
        }
        // Behavior 끼어들기(이벤트 등 처리)
        public void Interrupt(IBehavior behavior)
        {
            _list.AddFirst(behavior);
        }

        public void Run()
        {
            // 큐의 연쇄반응을 시작하는 함수
            TryRunNext();
        }

        // 도미노 밀기 시작
        private void TryRunNext()
        {
            // 리스트에 아무것도 없다면 그냥 return
            if (_list.First == null) return;
            // 실행 시작
            IBehavior cmd = _list.First.Value;
            _list.RemoveFirst();

            // 커맨드 실행! (끝나면 OnCommandFinished 호출)
            cmd?.Execute(OnBehaviorFinished);
        }

        // 커맨드가 완료될 때 콜백으로 들어가는 함수
        private void OnBehaviorFinished()
        {
            TryRunNext(); // 다음 도미노 넘김
        }
    }
}