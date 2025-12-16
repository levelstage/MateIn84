using GfEngine.Battles.Entities;

namespace GfEngine.Inputs;

public class BattleInputContext
{
    public Unit? Caster { get; set; }
    public bool IsCancelled { get; set; } = false;
    public bool IsConfirmed { get; set; } = false;
    public int SelectedSkillIndex { get; set; } = -1;
    public List<(int, int)> SelectedCoordinates { get; set; } = new();

    public BattleInputContext(Unit? caster)
    {
        Caster = caster;
    }

    public void Reset(Unit? caster)
    {
        Caster = caster;
        IsCancelled = false;
        SelectedSkillIndex = -1;
        SelectedCoordinates.Clear();
    }
}