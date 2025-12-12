using System;
using System.Collections.Generic;
using System.Linq;
using GfEngine.Battles.Interfaces;
using GfEngine.Battles.Units;
using GfEngine.Battles.Commands;

namespace GfEngine.Battles.Systems
{
    public class BattleManager
    {
        public static BattleManager Instance { get; private set; } = new BattleManager();

        public IBattleVisualizer? Visualizer { get; private set; }
        public CommandQueue Queue { get; private set; } = new CommandQueue();
        
        public List<Unit> AllUnits { get; private set; } = new List<Unit>();
        public Unit? CurrentUnit { get; private set; }

        public void Initialize(IBattleVisualizer visualizer)
        {
            Visualizer = visualizer;
            Visualizer.Initialize();
        }

        // 전투 시작
        public void StartBattle(List<Unit> units)
        {
            AllUnits = units;
            foreach (var u in AllUnits) u.InitializeAV();

            Console.WriteLine("⚔️ 전투 시작! 턴 계산에 들어갑니다.");
            ProcessNextTurn(); // 첫 턴 계산
        }

        // [핵심] 큐가 비었을 때 호출됨 (이전 유닛 행동 종료)
        public void OnQueueEmpty()
        {
            // 승패 판정 로직 등...
            
            // 다음 턴 진행
            ProcessNextTurn();
        }

        // 스타레일 식 턴 계산 (Update 루프 없이 수학적으로 시간 건너뜀)
        private void ProcessNextTurn()
        {
            // 1. 가장 AV가 작은(빨리 턴이 오는) 유닛 찾기
            var nextUnit = AllUnits
                .Where(u => !u.IsDead)
                .OrderBy(u => u.CurrentAG)
                .First();

            // 2. 시간 흐름 적용 (Time Warp)
            double timePassed = nextUnit.CurrentAG;
            foreach (var u in AllUnits)
            {
                if (!u.IsDead) u.CurrentAG -= timePassed;
            }

            // nextUnit의 AV는 이제 0이 됨
            CurrentUnit = nextUnit;
            CurrentUnit.ResetAG(); // 다음 턴을 위해 줄 서기

            // 3. 행동 결정
            if (CurrentUnit.Type == UnitType.Enemy)
            {
                // 적 AI: 즉시 커맨드 생성
                RunEnemyAI(CurrentUnit);
            }
            else
            {
                // 플레이어: 입력 대기 (여기선 시뮬레이션을 위해 자동 공격 처리)
                // 실제론 여기서 UI 띄우고 함수 종료 -> 나중에 Input 받아서 Enqueue
                Console.WriteLine($"🎮 [Player Turn] {CurrentUnit.Name}의 명령을 기다립니다...");
                SimulatePlayerInput(CurrentUnit); 
            }
        }

        // (테스트용) 적 AI
        private void RunEnemyAI(Unit enemy)
        {
            Console.WriteLine($"[Enemy Turn] {enemy.Name} 행동 시작!");
            
            // [패턴] 이동 -> 공격 -> 종료
            Queue.Enqueue(new VisualWaitCommand("Move", enemy) );
            Queue.Enqueue(new VisualWaitCommand("Attack", enemy));
            Queue.Enqueue(new ActionCommand(() => 
            {
                Console.WriteLine($"   => {enemy.Name}가 플레이어를 공격했습니다! (Logic)");
            }));
            // 큐에 넣는 순간 자동으로 실행 시작됨
        }

        // (테스트용) 플레이어 입력 시뮬레이션
        private void SimulatePlayerInput(Unit player)
        {
            // 1초 뒤에 플레이어가 '공격' 버튼을 눌렀다고 가정
            System.Threading.Tasks.Task.Delay(1000).ContinueWith(t => 
            {
                Queue.Enqueue(new VisualWaitCommand("Skill_Fireball", player));
                Queue.Enqueue(new ActionCommand(() => Console.WriteLine("   => 파이어볼 발사!")));
                // 큐에 넣으면 엔진 다시 돌아감
            });
        }
    }
}