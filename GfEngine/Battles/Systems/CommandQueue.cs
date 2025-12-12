using System;
using System.Collections.Generic;

namespace GfEngine.Battles.Systems
{
    // 모든 커맨드의 규격
    public interface ICommand
    {
        // 실행 시 '완료 보고용 콜백'을 받음
        void Execute(Action onComplete);
    }

    public class CommandQueue
    {
        private LinkedList<ICommand> _list = new LinkedList<ICommand>();
        private bool _isRunning = false;

        // 커맨드 추가 (도미노 세우기)
        public void Enqueue(ICommand cmd)
        {
            _list.AddLast(cmd);
            TryRunNext();
        }

        // 도미노 밀기 시작
        private void TryRunNext()
        {
            // 이미 돌아가는 중이면 건드리지 않음
            if (_isRunning) return;

            // 큐가 비었으면 -> 매니저에게 "턴 끝났음" 보고
            if (_list.Count == 0)
            {
                BattleManager.Instance.OnQueueEmpty();
                return;
            }

            // 실행 시작
            _isRunning = true;
            var cmd = _list.First?.Value;
            _list.RemoveFirst();

            // 커맨드 실행! (끝나면 OnCommandFinished 호출)
            cmd?.Execute(OnCommandFinished);
        }

        // 커맨드가 완료되었다고 보고함
        private void OnCommandFinished()
        {
            _isRunning = false;
            TryRunNext(); // 다음 도미노 넘김
        }
    }
}