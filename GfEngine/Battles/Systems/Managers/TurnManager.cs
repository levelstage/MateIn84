using GfEngine.Battles.Core;
using GfEngine.Battles.Entities;
using GfEngine.Core;
using GfEngine.Systems.Commands;

namespace GfEngine.Battles.Managers
{
    public class TurnManager
    {
        private static TurnManager? _instance;
        public static TurnManager Instance => _instance ??= new TurnManager();
        public Unit? CurrentUnit { get; private set; }

        // 전투 시작 함수. 임시이고, 원래는 DungeonNode 파일을 읽어와서 던전부터 생성해야 함.
        public void StartBattle(List<Unit> units)
        {
            foreach (var u in units) u.InitializeAG();
            ProcessNextTurn(); // 첫 턴 계산
        }

        // CTB 턴 계산 (Update 루프 없이 수학적으로 시간 건너뜀)
        private void ProcessNextTurn()
        {
            // 1. 가장 AG가 작은(빨리 턴이 오는) 유닛 찾기
            var nextUnit = EntityManager.Instance.AllUnits
                .Where(u => !u.IsDead)
                .OrderBy(u => u.CurrentAG)
                .First();

            // 2. 시간 흐름 적용 (Time Warp)
            double timePassed = nextUnit.CurrentAG;
            foreach (var u in EntityManager.Instance.AllUnits)
            {
                if (!u.IsDead) u.CurrentAG -= timePassed;
            }

            CurrentUnit = nextUnit;
            OnTurnStart();
        }
        // 
        private void ProcessTurn()
        {
            if(CurrentUnit == null) throw new Exception("Turn is processed without Unit.");
            if(CurrentUnit.Controller == null) throw new Exception("Unit must process turn with Controller");
            Sequence mainPhase = new Sequence();
            mainPhase.Enqueue(CurrentUnit.Controller.MakeBehavior(CurrentUnit));
            mainPhase.Execute(OnTurnEnd);
        }

        private void OnTurnStart()
        {
            // 여기서 턴 시작 처리를 해주고, 콜백으로 ProcessTurn을 넘긴다.
        }

        private void OnTurnEnd()
        {
            // 여기서 턴 종료 처리를 해주고, 콜백으로 ProcessNextTurn을 넘긴다.
        }


        // [Helper] 현재 유닛들의 행동 순서를 알려주는 함수
        // 반환 타입을 (string ID, double AG)로 명시하여 가독성 확보
        public List<(string ID, double AG)> GetUnitOrder()
        {
            // EntityManager.Instance.AllUnits가 IEnumerable<Unit>이라고 가정
            return EntityManager.Instance.AllUnits
                .Select(unit => (unit.ID, unit.CurrentAG)) // 필요한 데이터만 뽑기
                .OrderBy(t => t.CurrentAG) // AG 오름차순 정렬 (작은 게 먼저)
                .ToList();
        }

        // [Helper] 어떤 유닛이 행동한 후 다음 행동 순서 예측 함수
        public List<(string, double)> PredictNextOrder(Unit caster, TurnLength length)
        {
            // 1. Caster가 행동 후 갖게 될 새로운 AG 계산
            double nextAG = (double)length / caster.Speed;

            // 2. 전체 유닛 중에서 Caster만 쏙 빼고 가져오기
            var order = EntityManager.Instance.AllUnits
                .Where(u => u.ID != caster.ID) // Caster 제외시키기
                .Select(u => (u.ID, u.CurrentAG))
                .ToList();

            // 3. Caster의 미래 모습을 리스트에 추가
            // (보통 행동 후에는 현재 누적된 AG는 0이 되고 딜레이만 남는다고 가정)
            order.Add((caster.ID, nextAG));

            // 4. 다시 정렬해서 반환
            return order.OrderBy(t => t.CurrentAG).ToList();
        }
    }
}