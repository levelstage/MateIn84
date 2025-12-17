namespace GfEngine.Core
{
    // 시스템 내에서 커맨드를 처리하는 최소 단위. 시스템은 커맨드를 직접 처리하지 않고, 항상 Behavior를 거쳐가야 한다.
    public interface IBehavior
    {
        // 실행 시 '완료 보고용 콜백'을 받음
        // 또한 큐에 들어가지 않고, 즉시 실행되는것도 가능하다.
        void Execute(Action onComplete);
        // 내부적으로 커맨드 큐가 구현되어 있어야 함.
        void Enqueue(ICommand command);
    }
}