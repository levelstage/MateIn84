using GfEngine.Battles.Core;

namespace GfEngine.Battles.Augments
{
    public abstract class Augment : BattleElement
    {
        public string? OwnerID { get; set; }
        public string Description { get; set;} = "";
        public Augment() : base() {}
        public Augment(Augment o) : base(o)
        {
            OwnerID = o.OwnerID;
            Description = o.Description;
        }
    }
}