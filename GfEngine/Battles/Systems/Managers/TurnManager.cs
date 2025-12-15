using System;
using System.Collections.Generic;
using System.Linq;
using GfEngine.Battles.Interfaces;
using GfEngine.Battles.Entities;
using GfEngine.Inputs;

namespace GfEngine.Battles.Systems
{
    public class TurnManager
    {
        public static TurnManager Instance { get; private set; } = new TurnManager();

        public IBattleVisualizer? Visualizer { get; private set; }
        public CommandQueue Queue { get; private set; } = new CommandQueue();
        
        public List<Unit> AllUnits { get; private set; } = new List<Unit>();
        public Unit? CurrentUnit { get; private set; }
        private BattleInputContext inputContext = new BattleInputContext(null);

        public void Initialize(IBattleVisualizer visualizer)
        {
            Visualizer = visualizer;
            Visualizer.Initialize();
        }

        // 전투 시작
        public void StartBattle(List<Unit> units)
        {
            AllUnits = units;
            foreach (var u in AllUnits) u.InitializeAG();
            inputContext = new BattleInputContext(null); // 입력 컨텍스트 초기화.
            ProcessNextTurn(); // 첫 턴 계산
        }

        // 큐가 비었을 때 호출됨 (이전 유닛 행동 종료)
        public void OnQueueEmpty()
        {
            ProcessNextTurn();
        }

        // CTB 턴 계산 (Update 루프 없이 수학적으로 시간 건너뜀)
        private void ProcessNextTurn()
        {
            // 1. 가장 AG가 작은(빨리 턴이 오는) 유닛 찾기
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

            CurrentUnit = nextUnit;

            // 3. 행동 결정
            if (CurrentUnit.Type == UnitType.Enemy)
            {
                // 적 AI: 즉시 커맨드 생성
            }
            else
            {
                // 플레이어: 입력 대기 (여기선 시뮬레이션을 위해 자동 공격 처리)
                // 실제론 여기서 UI 띄우고 함수 종료 -> 나중에 Input 받아서 Enqueue
                
            }
            Queue.Enqueue(new ActionCommand(() =>
            {
                
            }));
        }

        // [Helper] 기존 순서 리스트와 다음 행동의 길이를 넣으면 0번 캐릭터의 다음 순서를 예측해주는 함수.
        public List<Unit> PredictNextOrder(List<Unit> prev, TurnLength length)
        {
            // 0. 리스트가 비어있거나 1명뿐인 경우 처리
            if (prev == null || prev.Count == 0) return new List<Unit>();
            
            List<Unit> res = new List<Unit>(prev);
            Unit unit = res[0];
            
            // 1. 다음 행동 후 예상 AG 계산
            // (CurrentAG를 사용하지 않고 순수하게 다음 턴에 걸릴 시간(AG)만 계산하는 것이 맞다면 unit.CurrentAG를 더해야 합니다.)
            // *주의: SRG 턴제 로직에 따라 unit.CurrentAG를 더할지 말지 결정해야 합니다.
            // 여기서는 기존 코드와 동일하게 length/speed만 계산하는 것으로 가정하고 진행합니다.
            double new_ag = (double)length / unit.Speed; 
            
            res.RemoveAt(0); // 대기열에서 제거

            // 2. [UpperBound 이진 탐색 시작]
            int left = 0;
            int right = res.Count; // Count-1이 아니라 Count로 설정합니다. (삽입 위치를 포함하기 위함)

            while (left < right)
            {
                int mid = left + (right - left) / 2;

                // 찾는 값(new_ag)보다 작거나 같다면, 나는 mid보다 오른쪽에 위치해야 한다.
                // (값이 같을 때도 뒤로 밀려나는 UpperBound 로직)
                if (res[mid].CurrentAG <= new_ag)
                {
                    left = mid + 1;
                }
                else // 찾는 값보다 크다면, mid가 후보이므로 right를 mid로 줄인다.
                {
                    right = mid;
                }
            }
            // [UpperBound 이진 탐색 끝]
            
            // 3. left(==right)가 삽입해야 할 정확한 위치입니다.
            // 이 값은 0부터 res.Count까지 보장됩니다.
            res.Insert(left, unit); 

            return res;
        }
    }
}