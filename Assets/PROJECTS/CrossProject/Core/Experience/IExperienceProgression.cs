namespace CrossProject.Core.Experience
{
    public interface IExperienceProgression
    {
        int StartingLevel { get; }
        int MaxLevel { get; }
        int ExperienceToNextLevel(int currentLevel, int currentExperiencePoints);
    }
}
