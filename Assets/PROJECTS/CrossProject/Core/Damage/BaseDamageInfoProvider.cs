namespace CrossProject.Core
{
    public interface IDamageInfoProvider
    {
        float Damage { get; }
    }
    
    public abstract class BaseDamageInfoProvider : IDamageInfoProvider
    {
        public abstract float Damage { get; }
    }
}
