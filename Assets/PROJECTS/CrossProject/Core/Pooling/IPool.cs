namespace CrossProject.Core.Pooling
{
    public interface IPool
    {
        void Return(IPoolElement element);
    }
}