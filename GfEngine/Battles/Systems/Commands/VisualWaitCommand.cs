using System;
using GfEngine.Battles.Systems;
using GfEngine.Battles.Entities;

namespace GfEngine.Battles.Systems
{
    // 연출을 기다리는 커맨드
    public class VisualWaitCommand : ICommand
    {
        private string _animName;
        private Unit _target;

        public VisualWaitCommand(string animName, Unit target)
        {
            _animName = animName;
            _target = target;
        }

        public void Execute(Action onComplete)
        {
            // 비주얼라이저에게 외주 맡김.
            // "연출 끝나면 onComplete 좀 대신 눌러주쇼" 하고 엔진은 빠짐.
            if(TurnManager.Instance.Visualizer == null)
            {
                onComplete?.Invoke();
                return;
            }
        }

        public string GetLog()
        {
            return "";
        }
    }
}