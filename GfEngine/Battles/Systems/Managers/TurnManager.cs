using System.Linq;
using GfEngine.Battles.Core;
using GfEngine.Battles.Entities;
using GfEngine.Battles.Systems;
using GfEngine.Core.Commands;

namespace GfEngine.Battles.Managers
{
    public class TurnManager
    {
        private static TurnManager? _instance;
        public static TurnManager Instance => _instance ??= new TurnManager();
        public Unit? CurrentUnit { get; set; }
        private IEnumerable<IAGHolder> AllAGHolders
        {
            get
            {
                var entities = EntityManager.Instance.AllEntities.Cast<IAGHolder>();
                var augments = AugmentManager.Instance.AllAugments.Cast<IAGHolder>();
                return entities.Concat(augments);
            }
        }

        // 전투 시작 함수. 임시이고, 원래는 DungeonNode 파일을 읽어와서 던전부터 생성해야 함.
        public void StartBattle()
        {
            foreach (var u in AllAGHolders) u.InitializeAG();
            PassTurn();
        }

        // CTB 턴 계산 (Update 루프 없이 수학적으로 시간 건너뜀)
        private void PassTurn()
        {
            // 1. 가장 AG가 작은(빨리 턴이 오는) 유닛 찾기
            var nextOne = AllAGHolders
                .Where(h => !h.IsDead)
                .OrderBy(u => (double)u.CurrentAG/u.Speed)
                .First();

            // 2. 시간 흐름 적용 (Time Warp)
            double timePassed = (double)nextOne.CurrentAG / nextOne.Speed;
            foreach (var h in AllAGHolders)
            {
                if (!h.IsDead) h.CurrentAG -= (int)Math.Ceiling(timePassed * h.Speed);
            }
            nextOne.OnTurnStart(PassTurn);
        }
        // 

        private void OnTurnEnd()
        {
            // 귀찮으니까 나중에 만듬
        }


        // [Helper] 현재 유닛들의 행동 순서를 알려주는 함수
        public List<(string, double)> GetUnitOrder()
        {
            return EntityManager.Instance.AllUnits
                .Select(unit => (unit.ID, (double)unit.CurrentAG / unit[StatType.Speed])) // 필요한 데이터만 뽑기
                .OrderBy(unit => unit.Item2) // AG 오름차순 정렬 (작은 게 먼저)
                .ToList();
        }

        // [Helper] 어떤 유닛이 행동한 후 다음 행동 순서 예측 함수
        public List<(string, double)> PredictNextOrder(Unit caster, TurnLength length)
        {
            // 1. Caster가 행동 후 갖게 될 새로운 남은 턴 수 계산
            double requiredTurn = (double)length / caster[StatType.Speed];

            // 2. 전체 유닛 중에서 Caster만 쏙 빼고 가져오기
            var order = EntityManager.Instance.AllUnits
                .Where(u => u.ID != caster.ID) // Caster 제외시키기
                .Select(u => (u.ID, (double)u.CurrentAG / u[StatType.Speed]))
                .ToList();

            // 3. Caster의 미래 모습을 리스트에 추가
            // (보통 행동 후에는 현재 누적된 AG는 0이 되고 딜레이만 남는다고 가정)
            order.Add((caster.ID, requiredTurn));

            // 4. 다시 정렬해서 반환
            return order.OrderBy(t => t.Item2).ToList();
        }
    }
}