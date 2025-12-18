using GfEngine.Battles.Core;

namespace GfEngine.Battles.Augments
{
    public class Status : Augment
    {
        public int this[StatType type] => GetValue(type);
        
        private readonly Dictionary<StatType, int> _statMap = new();
        private readonly Dictionary<StatType, List<StatModifier>> _modifiers = new();

        // 더티 플래그를 이용한 캐싱
        // _cachedValues: 계산이 끝난 최종 값을 저장해두는 곳
        private readonly Dictionary<StatType, int> _cachedValues = new();
        // _isDirty: "값이 바뀌어서 다시 계산해야 함?" 여부를 체크 (true면 다시 계산)
        private readonly Dictionary<StatType, bool> _isDirty = new();

        public Status()
        {
            // 초기화
        }

        private int RecalculateStat(StatType type)
        {
            // TryGetValue를 써서 KeyNotFound 에러 방지 및 기본값 0 처리
            int @base = _statMap.TryGetValue(type, out int v) ? v : 0;

            // 수정자가 없으면 계산할 필요 없이 원본 리턴
            if (!_modifiers.TryGetValue(type, out var mods) || mods.Count == 0)
                return @base;

            int flat = 0;
            double pAdd = 0.0; // 100% = 1.0 (기본값 0.0부터 시작)
            double pMult = 1.0; // 곱연산은 1.0부터 시작
            foreach (var m in mods)
            {
                switch (m.Type)
                {
                    // m.Value는 double이다.
                    case StatModType.Flat: 
                        flat += (int)m.Value; 
                        break;
                    case StatModType.PercentAdd: 
                        pAdd += m.Value / 100;  
                        break; 
                    case StatModType.PercentMult: 
                        pMult *= m.Value / 100;
                        break;
                }
            }
            return (int)((@base + flat + (@base * pAdd)) * pMult);
        }

        public int GetValue(StatType type, bool isOrigin = false)
        {
            // 1. 원본 요청이면 그냥 줌
            if (isOrigin) return _statMap.TryGetValue(type, out int v) ? v : 0;

            // 2. 더티 체크: 값이 "더러워졌으면(Dirty)" 다시 계산
            if (!_isDirty.TryGetValue(type, out bool dirty) || dirty)
            {
                _cachedValues[type] = RecalculateStat(type);
                _isDirty[type] = false; // "이제 깨끗함(계산 끝)"
            }

            // 3. 저장된 값 리턴 (빠름!)
            return _cachedValues[type];
        }

        public void AddModifier(StatModifier mod)
        {
            if (!_modifiers.ContainsKey(mod.TargetStat))
            {
                _modifiers[mod.TargetStat] = new();
            }
            _modifiers[mod.TargetStat].Add(mod);

            // 값이 바뀌었으므로 "더러움" 표시 -> 다음 GetValue 때 재계산함
            _isDirty[mod.TargetStat] = true; 
        }

        public void RemoveModifier(StatModifier mod)
        {
            if (_modifiers.TryGetValue(mod.TargetStat, out var list))
            {
                if (list.Remove(mod))
                {
                    // 지워지는것도 값을 바꾸는 행위이니 Dirty.
                    _isDirty[mod.TargetStat] = true;
                }
            }
        }
        // 아이템 착용 등으로 인한 BaseStat 변경시.
        public void SetBaseStat(StatType type, int value)
        {
            _statMap[type] = value;
            _isDirty[type] = true;
        }
    }
}