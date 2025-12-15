using GfEngine.Battles.Entities;

namespace GfEngine.Battles.Systems
{
    // 전투 관련 이벤트의 최상위 부모
    public abstract class BattleEventData { }

    // [상황 1: 치고 박고 싸울 때]
    public class CombatEventData : BattleEventData
    {
        public Unit Attacker;
        public Unit Defender;
        public int Damage;

        public CombatEventData(Unit atk, Unit def, int dmg)
        {
            Attacker = atk; Defender = def; Damage = dmg;
        }
    }

    // [상황 2: 턴이 넘어갈 때]
    public class TurnEventData : BattleEventData
    {
        public Unit TargetUnit; // 누구 턴인가?
        public int TurnCount;

        public TurnEventData(Unit unit, int count = 0)
        {
            TargetUnit = unit; TurnCount = count;
        }
    }
}