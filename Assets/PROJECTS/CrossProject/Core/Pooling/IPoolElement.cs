namespace CrossProject.Core.Pooling
{
    public interface IPoolElement
    {
        void SetPool(IPool pool);
        void OnGet();
        void OnReturn();
        bool IsAvailableToGet { get; }
    }
}