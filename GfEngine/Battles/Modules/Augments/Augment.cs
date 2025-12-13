using GfEngine.Battles.Modules.Entities;

namespace GfEngine.Battles.Modules.Augments
{
    public abstract class Augment
    {
        public Entity? Owner { get; set; }
        public string Description { get; set;} = "";
    }
}