using GfEngine.Battles.Entities;
namespace GfEngine.Battles.Managers
{
    public class EntityManager
    {
        private static EntityManager? _instance;
        public static EntityManager Instance => _instance ??= new EntityManager();
        private readonly Dictionary<string, Entity> _allEntities = new Dictionary<string, Entity>();
        private readonly Dictionary<string, int> _nextInstanceNum = new Dictionary<string, int>();
        public IEnumerable<Unit> AllUnits
        {
            get{
                return _allEntities.Values.OfType<Unit>(); 
            }
        }

        public Unit GetUnit(string id)
        {
            if (_allEntities.TryGetValue(id, out Entity? entity))
            {
                if (entity is Unit unit) return unit;
                throw new Exception($"Entity {id} exists but is not a Unit.");
            }
            throw new Exception($"Unit with ID {id} not found.");
        }
        private int GetInstanceNum(string code)
        {
            if(!_nextInstanceNum.ContainsKey(code)) _nextInstanceNum[code] = 1;  // 0번은 데이터베이스상의 데이터로 간주.
            return _nextInstanceNum[code]++;
        }
        internal void Spawn(Entity entity)
        {
            // 원래는 맵 상에 엔티티를 배치하는 로직도 들어가야 함.
            entity.InstanceNum = GetInstanceNum(entity.Code);
        }
    }
}