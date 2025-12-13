namespace GfEngine.Core
{
    public abstract class GameElement
    {
        public required string ID { get; set; }
        public string Name { get; set; } = "Unknown";
    }
}