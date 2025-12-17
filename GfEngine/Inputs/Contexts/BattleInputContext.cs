using System.Security.Cryptography.X509Certificates;
using GfEngine.Battles.Entities;

namespace GfEngine.Inputs;

public class BattleInputContext
{
    public Unit? Caster { get; set; }
    public bool IsCancelled { get; set; } = false;
    public bool IsConfirmed { get; set; } = false;
    public string SelectedSkillID { get; set; } = "NONE";
    public List<(int, int)> SelectedCoordinates { get; set; } = new();

    public BattleInputContext() {}

    public BattleInputContext(Unit? caster)
    {
        Caster = caster;
    }

    public BattleInputContext(BattleInputContext o)
    {
        Caster = o.Caster;
        IsCancelled = o.IsCancelled;
        IsConfirmed = o.IsConfirmed;
        SelectedSkillID = o.SelectedSkillID;
        SelectedCoordinates = new List<(int, int)>(o.SelectedCoordinates);
    }
}