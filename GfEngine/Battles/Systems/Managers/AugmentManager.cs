using GfEngine.Battles.Augments;
namespace GfEngine.Battles.Managers
{
    public class AugmentManager
    {
        private static AugmentManager? _instance;
        public static AugmentManager Instance => _instance ??= new AugmentManager();
        private readonly Dictionary<string, Augment> _allAugments = [];
        private readonly Dictionary<string, int> _nextInstanceNum = new Dictionary<string, int>();
        public IEnumerable<Augment> AllAugments => _allAugments.Values;
        public IEnumerable<Skill> AllSkills => _allAugments.Values.OfType<Skill>(); 
    
        public Skill GetSkill(string id)
        {
            if (_allAugments.TryGetValue(id, out Augment? augment))
            {
                if (augment is Skill skill) return skill;
                throw new Exception($"Entity {id} exists but is not a Skill.");
            }
            throw new Exception($"Skill with ID {id} not found.");
        }
        public Augment GetAugment(string id)
        {
            if(_allAugments.TryGetValue(id, out var augment))
            {
                return augment;
            }
            throw new Exception($"Entity with ID {id} not found.");
        }
        private int GetInstanceNum(string code)
        {
            if(!_nextInstanceNum.ContainsKey(code)) _nextInstanceNum[code] = 1;  // 0번은 데이터베이스상의 데이터로 간주.
            return _nextInstanceNum[code]++;
        }
        internal void Spawn(Augment entity)
        {
            // 원래는 맵 상에 엔티티를 배치하는 로직도 들어가야 함.
            entity.InstanceNum = GetInstanceNum(entity.Code);
        }
    }
}