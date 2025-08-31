namespace CrossProject.Core.Experience
{
    public interface IExperienceProvider
    {
        int CurrentTotalExp { get; }
        int CurrentLevel { get; }
        void AddExp(int amount);
    }
}
