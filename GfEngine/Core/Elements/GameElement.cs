namespace GfEngine.Core
{
    public abstract class GameElement
    {
        public string Code { get; set; } = "Unknown";
        public string Name { get; set; } = "Unknown";
        public int InstanceNum { get; set; } = 0;
        public string ID => $"{Code}#{InstanceNum}";
        public GameElement()
        {
            Code = "Unknown";
            Name = "Unknown";
        }
        public GameElement(GameElement o)
        {
            Code = o.Code;
            Name = o.Name;
        }
    }
}