namespace GfEngine.Core;
public class Behavior : IBehavior
{
    private readonly Queue<ICommand> _queue;  // 처리할 커맨드들을 순차적으로 담아둔 큐
    private Action? _onComplete;  // Execute 시점에서 받은 콜백을 저장하는 함수
    public Behavior()
    {
        // 생성시에 빈 큐를 생성함.
        _queue = new Queue<ICommand>();
    }
    public Behavior(ICommand command)
    {
        // 처리할 일이 커맨드 하나더라도, 시스템은 행동만 큐에 넣을 수 있기 때문에 행동으로 변환해주는 생성자가 있어야함.
        _queue = new Queue<ICommand>();
        _queue.Enqueue(command);
    }
    // Behavior의 메인 로직. 재귀적 콜백 호출을 통해, 순서가 보장되는 비동기 처리로 큐를 밀 수 있음.
    private void TryRunNext()
    {
        if(_queue.Count == 0)
        {
            // 큐의 모든 명령을 처리한 후 콜백을 실행한다.
            _onComplete?.Invoke();
            return;
        }
        ICommand command = _queue.Dequeue();
        command.Execute(TryRunNext);
        return;
    }
    // 실행 부분. 콜백은 모든 처리가 끝난 후 실행하기 위해 담아둔다. (IBehavior 인터페이스)
    public void Execute(Action onComplete)
    {
        _onComplete = onComplete;
        TryRunNext();
    }
    // 실행할 커맨드 목록에 새로운 커맨드를 추가한다. (IBehavior 인터페이스)
    public void Enqueue(ICommand command)
    {
        _queue.Enqueue(command);
    }
}