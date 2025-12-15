using GfEngine.Battles.Entities;

namespace GfEngine.Battles.Augments
{
    public abstract class Augment
    {
        public Entity? Owner { get; set; }
        public string Description { get; set;} = "";
    }
}