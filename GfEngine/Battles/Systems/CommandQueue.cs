using System;
using System.Collections.Generic;
using GfEngine.Battles.Commands;

namespace GfEngine.Battles.Systems
{
    public class CommandQueue
    {
        // 명령 대기열 (FIFO: 먼저 들어온 게 먼저 실행됨)
        private Queue<ICommand> _queue = new Queue<ICommand>();

        // 1. 명령 접수
        public void Enqueue(ICommand cmd)
        {
            _queue.Enqueue(cmd);
        }

        // 2. 하나 꺼내서 처리 (애니메이션 동기화용)
        public void ProcessNext()
        {
            if (_queue.Count > 0)
            {
                ICommand cmd = _queue.Dequeue();
                
                // 로그 찍고 실행
                Console.WriteLine($"[CMD] {cmd.GetLog()}");
                cmd.Execute();
            }
        }

        // 3. 싹 다 처리 (빠른 결과 확인용)
        public void ProcessAll()
        {
            while (_queue.Count > 0)
            {
                ProcessNext();
            }
        }
    }
}