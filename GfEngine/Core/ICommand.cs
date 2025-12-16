namespace GfEngine.Core
{
    // 모든 커맨드의 규격
    public interface ICommand
    {
        // 실행 시 '완료 보고용 콜백'을 받음
        void Execute(Action onComplete);
    }
}