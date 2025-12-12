using GfEngine.Battles.Units;

namespace GfEngine.Battles.Commands
{
    // [1] 데미지 명령
    public class DamageCommand : ICommand
    {
        private Unit _attacker; // 때린 놈 (독뎀이면 null일 수도 있음)
        private Unit _target;   // 맞는 놈
        private int _damage;    // 데미지 양

        public DamageCommand(Unit attacker, Unit target, int damage)
        {
            _attacker = attacker;
            _target = target;
            _damage = damage;
        }

        public void Execute()
        {
            // 실제 유닛의 체력을 깎는 로직
            // (방어력 적용은 보통 Command 생성 전에 계산해서 넘겨주거나, 여기서 계산함)
            // 여기서는 심플하게 이미 계산된 데미지가 들어왔다고 가정
            _target.CurrentHP -= _damage;
            
            // 사망 체크 등은 여기서 하거나 유닛 내부에서 트리거
            if (_target.CurrentHP < 0) _target.CurrentHP = 0;
        }

        public string GetLog()
        {
            string atkName = _attacker != null ? _attacker.Name : "System";
            return $"{atkName} -> {_target.Name}에게 {_damage} 피해";
        }
    }

    // [2] 힐 명령
    public class HealCommand : ICommand
    {
        private Unit _target;
        private int _amount;

        public HealCommand(Unit target, int amount)
        {
            _target = target;
            _amount = amount;
        }

        public void Execute()
        {
            _target.CurrentHP += _amount;
            if (_target.CurrentHP > _target.CombatStats.MaxHP)
                _target.CurrentHP = _target.CombatStats.MaxHP;
        }

        public string GetLog() => $"{_target.Name} 체력 {_amount} 회복";
    }
}