using GfEngine.Core;

namespace GfEngine.Battles.Core
{
    public abstract class BattleElement : GameElement
    {
        public BattleElement() : base(){}
        public BattleElement(BattleElement o) : base(o) {}
    }    
}