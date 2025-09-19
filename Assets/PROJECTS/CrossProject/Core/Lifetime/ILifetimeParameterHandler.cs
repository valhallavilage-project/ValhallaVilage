namespace CrossProject.Core
{
    public interface ILifetimeParameterHandler : IBoxedValueHandler<float>
    {
        bool IsFullyRestored { get; }
        void Restore(float value);
    }
}
