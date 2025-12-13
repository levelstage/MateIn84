using System;
using GfEngine.Battles.Modules.Entities.Units;

namespace GfEngine.Battles.Interfaces
{
    // 엔진이 클라이언트에게 바라는 요구사항
    public interface IBattleVisualizer
    {
        void Initialize();

        // [Blocking] 엔진을 멈추고 연출을 보여달라. 다 되면 onComplete를 눌러라.
        void PlayAnimation(string animName, Unit target, Action onComplete);

        // [Non-Blocking] 이펙트나 텍스트를 띄워라. (엔진은 안 기다림)
        void TriggerEffect(string effectName, Unit target);
    }
}