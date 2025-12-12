namespace GfEngine.Battles.Commands
{
    public interface ICommand
    {
        // 실제로 로직을 수행하는 함수
        void Execute();

        // 디버깅용: "누가 누구를 때림" 같은 로그 출력
        string GetLog();
    }
}