using System.Net.Cache;
using GfEngine.Battles.Entities;

namespace GfEngine.Battles.Dungeons
{
    public abstract class DungeonNode
    {
        public required int[,] Tiles;
        public required Dictionary<int, TileMetadata> TileInfo;
        public required List<Entity> Entities;
        public int Height;
        public int Width;
        
    }
    public struct TileMetadata 
    {
        public bool IsWalkable { get; set; }
        public bool IsWater { get; set; }
        // ... 
    }
    
}