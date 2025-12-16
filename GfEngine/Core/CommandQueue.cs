namespace GfEngine.Core
{
    // 큐에 대한 접근은 엔진의 고유 권한이므로 internal.
    internal class CommandQueue
    {
        private static CommandQueue? _instance;
        public static CommandQueue Instance => _instance ??= new CommandQueue();
        private LinkedList<ICommand> _list = new LinkedList<ICommand>();

        // 커맨드 추가 (도미노 세우기)
        public void Enqueue(ICommand cmd)
        {
            _list.AddLast(cmd);
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
            ICommand cmd = _list.First.Value;
            _list.RemoveFirst();

            // 커맨드 실행! (끝나면 OnCommandFinished 호출)
            cmd?.Execute(OnCommandFinished);
        }

        // 커맨드가 완료될 때 콜백으로 들어가는 함수
        private void OnCommandFinished()
        {
            TryRunNext(); // 다음 도미노 넘김
        }
    }
}