using System;
using System.Collections.Generic;
using System.Threading;
using GfEngine.Battles.Systems;
using GfEngine.Battles.Units;
using GfEngine.Visualizers.ConsoleViz;

class Program
{
    static void Main(string[] args)
    {
        // 1. 세팅
        var visualizer = new ConsoleBattleVisualizer();
        BattleManager.Instance.Initialize(visualizer);

        // 2. 유닛 생성
        var units = new List<Unit>
        {
            new Unit { Name = "King(Player)", Type = UnitType.Player},
            new Unit { Name = "Hagen(Player)", Type = UnitType.Player,},
            new Unit { Name = "Goblin(Enemy)", Type = UnitType.Enemy,} // 빠름!
        };
        units[0].CombatStats.Speed = 100;
        units[1].CombatStats.Speed = 80;
        units[2].CombatStats.Speed = 120;

        // 3. 전투 시작
        BattleManager.Instance.StartBattle(units);

        // 4. 메인 루프 (클라이언트/OS 역할)
        // 엔진 업데이트는 없지만, 프로세스가 죽지 않게 잡아두는 역할
        while (true)
        {
            // 만약 렌더러가 Draw 루프가 필요하다면 여기서 호출
            // visualizer.Render(); 
            
            Thread.Sleep(100);
        }
    }
}