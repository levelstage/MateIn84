namespace GfEngine.Battles.Systems;

public interface IAGHolder
{
    public int CurrentAG { get; set; }
    public int Speed { get; }
    public bool IsDead { get; }
    public void InitializeAG();
    public void OnTurnStart(Action onComplete);
}