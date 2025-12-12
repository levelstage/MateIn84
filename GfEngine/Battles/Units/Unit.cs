using System;
using System.Collections.Generic;
using GfEngine.Battles.Systems;

namespace GfEngine.Battles.Units
{
    public enum UnitType { Player, Enemy }
    public class Unit
    {
        // === [기본 정보] ===
        public string Name { get; set; } = "Unknown";
        public UnitType Type { get; set; }
        public int Level { get; set; } = 1;

        // === [A. 스탯 관련 로직들] ===

        // === [스탯 데이터] ===
        // 1. 기초 스탯 (원본: 레벨업으로만 성장)
        public PrimaryStat BaseStats;

        // 2. 파생 스탯 (결과물: 매번 다시 계산됨)
        public DerivedStat CombatStats;

        // 3. 현재 상태 (HP, MP 등)
        public int CurrentHP;
        public int CurrentMP;
        public int CurrentPoise;
        public double CurrentAG;
        public bool IsDead => CurrentHP == 0;
        // AG 관련
        public void ResetAG()
        {
            CurrentAG = 10000.0 / CombatStats.Speed; 
        }

        // 초기화
        public void InitializeAV()
        {
            // 첫 턴은 랜덤성을 주거나 절반으로 시작
            CurrentAG = 10000.0 / CombatStats.Speed; 
        }

        // === [스탯 조작기 (Modifier)] ===
        public List<StatModifier> StatModifiers = new List<StatModifier>();

        // 스탯 종류 총 개수 (배열 크기용)
        private static readonly int _statTypeCount = Enum.GetNames(typeof(StatType)).Length;

        // === [Modifier 관리] ===
        public void AddModifier(StatModifier mod)
        {
            StatModifiers.Add(mod);
            UpdateStats();
        }

        public void RemoveModifier(StatModifier mod)
        {
            StatModifiers.Remove(mod);
            UpdateStats();
        }

        public void RemoveModifiersBySource(string sourceID)
        {
            StatModifiers.RemoveAll(m => m.SourceID == sourceID);
            UpdateStats();
        }

        // === [핵심 로직: 스탯 갱신 (합연산 방식)] ===
        public void UpdateStats()
        {
            // [Step 0] 임시 버퍼 초기화 (모든 보너스를 여기에 모음)
            // 인덱스: StatType의 정수값
            float[] flatSum = new float[_statTypeCount];
            float[] pctSum = new float[_statTypeCount];

            // [Step 1] 모든 Modifier 순회하며 버퍼에 합산
            foreach (var mod in StatModifiers)
            {
                int index = (int)mod.TargetStat;

                switch (mod.Type)
                {
                    case StatModType.Flat:
                        flatSum[index] += mod.Value;
                        break;
                        
                    case StatModType.PercentAdd:
                        // 예: 10% -> 0.1f로 변환해서 누적
                        pctSum[index] += (mod.Value / 100f); 
                        break;
                }
            }

            // [Step 2] 기초 스탯(Primary) 확정
            // 공식: (Base + Flat) * (1 + Sum%)
            PrimaryStat t = new PrimaryStat();
            
            t.Vit = CalcPrimary(BaseStats.Vit, StatType.Vit, flatSum, pctSum);
            t.End = CalcPrimary(BaseStats.End, StatType.End, flatSum, pctSum);
            t.Str = CalcPrimary(BaseStats.Str, StatType.Str, flatSum, pctSum);
            t.Agi = CalcPrimary(BaseStats.Agi, StatType.Agi, flatSum, pctSum);
            t.Dex = CalcPrimary(BaseStats.Dex, StatType.Dex, flatSum, pctSum);
            t.Luk = CalcPrimary(BaseStats.Luk, StatType.Luk, flatSum, pctSum);
            t.Spr = CalcPrimary(BaseStats.Spr, StatType.Spr, flatSum, pctSum);
            t.Mag = CalcPrimary(BaseStats.Mag, StatType.Mag, flatSum, pctSum);
            t.Int = CalcPrimary(BaseStats.Int, StatType.Int, flatSum, pctSum);

            // [Step 3] 파생 스탯(Derived) 변환 (기획된 공식 적용)
            DerivedStat final = new DerivedStat();

            // 3-1. 자원
            final.MaxHP = t.Vit * 2;
            final.MaxMP = t.Mag + t.Int;
            final.MaxPoise = t.End + t.Spr;      // 지구력 + 정신력
            final.MaxWeight = t.Str + t.End;     // 근력 + 지구력

            // 3-2. 공격
            final.AtkPhy = t.Str / 3;
            final.AtkMag = t.Mag / 3;
            final.Suppression = (t.Str * 2) + (t.Mag / 2);
            final.CritDmg = 150 + (t.Dex * 5);   // 기본 150% + 솜씨 보정

            // 3-3. 방어 & 유틸
            final.DefPhy = (t.Vit + t.End) / 10;
            final.DefMag = (t.Spr + t.Mag) / 10;
            final.CritResist = Math.Min(60, t.Luk * 5); // 최대 60%
            final.DropRateBonus = t.Luk * 2;


            // [Step 4] 파생 스탯 최종 보정 (Modifier 적용)
            // 여기도 똑같이 (값 + Flat) * (1 + Sum%)
            ApplyDerivedMod(ref final.MaxHP, StatType.MaxHP, flatSum, pctSum);
            ApplyDerivedMod(ref final.MaxMP, StatType.MaxMP, flatSum, pctSum);
            ApplyDerivedMod(ref final.AtkPhy, StatType.AtkPhy, flatSum, pctSum);
            ApplyDerivedMod(ref final.AtkMag, StatType.AtkMag, flatSum, pctSum);
            ApplyDerivedMod(ref final.DefPhy, StatType.DefPhy, flatSum, pctSum);
            ApplyDerivedMod(ref final.DefMag, StatType.DefMag, flatSum, pctSum);
            
            ApplyDerivedMod(ref final.MaxPoise, StatType.MaxPoise, flatSum, pctSum);
            ApplyDerivedMod(ref final.CritDmg, StatType.CritDmg, flatSum, pctSum);

            // 결과 저장
            CombatStats = final;
        }

        // [Helper] 기초 스탯 계산용
        private int CalcPrimary(int baseVal, StatType type, float[] flats, float[] pcts)
        {
            int idx = (int)type;
            float flat = flats[idx];
            float pct = pcts[idx]; 

            // (기본 + 고정합) * (1 + 퍼센트합)
            float result = (baseVal + flat) * (1.0f + pct);
            return (int)result;
        }

        // [Helper] 파생 스탯 적용용 (ref 사용)
        private void ApplyDerivedMod(ref int val, StatType type, float[] flats, float[] pcts)
        {
            int idx = (int)type;
            float flat = flats[idx];
            float pct = pcts[idx];

            float result = (val + flat) * (1.0f + pct);
            val = (int)result;
        }

        // [Helper] 상태 완전 회복 (테스트용)
        public void FullRestore()
        {
            CurrentHP = CombatStats.MaxHP;
            CurrentMP = CombatStats.MaxMP;
            CurrentPoise = CombatStats.MaxPoise;
        }

        // === [B. 이벤트 관련 로직들] ===
        
        private Dictionary<string, List<Action<BattleEventData>>> _subscribers 
            = new Dictionary<string, List<Action<BattleEventData>>>();

        // 1. 구독 (Subscribe)
        public void Subscribe(string eventName, Action<BattleEventData> callback)
        {
            if (!_subscribers.ContainsKey(eventName))
                _subscribers[eventName] = new List<Action<BattleEventData>>();

            _subscribers[eventName].Add(callback);
        }

        // 2. 취소 (Unsubscribe)
        public void Unsubscribe(string eventName, Action<BattleEventData> callback)
        {
            if (_subscribers.ContainsKey(eventName))
                _subscribers[eventName].Remove(callback);
        }

        // 3. 방송 (Publish)
        public void Publish(string eventName, BattleEventData data)
        {
            if (_subscribers.ContainsKey(eventName))
            {
                // 리스트 복사 후 순회 (안전성 확보)
                var callbacks = new List<Action<BattleEventData>>(_subscribers[eventName]);
                foreach (var callback in callbacks)
                {
                    callback.Invoke(data);
                }
            }
        }
    }
}