namespace GfEngine.Battles.Augments
{
    public class SkillDomain
    {
        public List<string>? ValidSkillIDs;
        public int XYCounts = 1;
        public List<(int, int)>? ValidXYs;
        public List<(int, int)>? RangedXYs;
    }
}