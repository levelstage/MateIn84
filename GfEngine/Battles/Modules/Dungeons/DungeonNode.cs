using System.Net.Cache;

namespace GfEngine.Battles.Modules.Dungeons
{
    public abstract class DungeonNode
    {
        public required int[,] Tiles;
        public required Dictionary<int, TileMetadata> TileInfo;
        public int Height;
        public int Width;
        public string[,]? Holder;
        // 아래는 Tiled에서 만들어둔 Object Lyaer의 Object들

        // List<Enemy> Enemies;
        // List<GroundEffect> GroundEffects;
        // ...
        
    }
    public struct TileMetadata 
    {
        public bool IsWalkable { get; set; }
        public bool IsWater { get; set; }
        // ... 
    }
    
}