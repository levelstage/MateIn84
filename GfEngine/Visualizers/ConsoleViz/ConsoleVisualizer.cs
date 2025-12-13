using System;
using System.Threading.Tasks;
using GfEngine.Battles.Interfaces;
using GfEngine.Battles.Modules.Entities.Units;

namespace GfEngine.Visualizers.ConsoleViz
{
    public class ConsoleBattleVisualizer : IBattleVisualizer
    {
        public void Initialize()
        {
            Console.Clear();
            Console.WriteLine("=== [Console Visualizer Ready] ===");
        }

        // 엔진의 요청을 받아서 연출 수행
        public async void PlayAnimation(string animName, Unit target, Action onComplete)
        {
            // 1. 연출 시작 로그
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[Visual] '{animName}' ({target.Name}) 재생 중...");
            Console.ResetColor();

            // 2. 시간 끌기 (애니메이션 길이 시뮬레이션: 1초)
            await Task.Delay(1000); 

            // 3. 연출 종료 로그
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[Visual] '{animName}' 완료.");
            Console.ResetColor();

            // 4. 엔진에게 보고 (큐 잠금 해제!)
            onComplete?.Invoke();
        }

        public void TriggerEffect(string effectName, Unit target)
        {
            Console.WriteLine($"[Effect] {effectName}!");
        }
    }
}