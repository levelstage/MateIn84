using GfEngine.Core;
using GfEngine.Battles.Modules.Augments.Items;
namespace GfEngine.Battles.Modules.Entities.Units.Actors
{
    // [변경] CharacterData -> ActorData
    // 플레이어블 캐릭터(Actor)의 영속적인 정보를 담습니다.
    public class ActorData : GameElement
    {
        // 레벨, 경험치, 스탯, 직업 등
        public int Level { get; set; }
        public int Exp { get; set; }
        
        // 인벤토리 (이 Actor가 소유한 장비 등)
        public List<Item> PersonalInventory { get; set; } = new List<Item>();
    }
}