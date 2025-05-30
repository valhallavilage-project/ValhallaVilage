namespace CrossProject.Core.Pooling
{
    public abstract class MonoPoolElement : CustomBehaviour, IPoolElement
    {
        public IPool Pool { get; private set; }

        public void SetPool(IPool pool)
        {
            Pool = pool;
        }

        protected override void OnGameObjectDestroy()
        {
            Pool.Return(this);
        }

        public virtual void OnGet()
        {
            gameObject.SetActive(true);
        }

        public virtual void OnReturn()
        {
            gameObject.SetActive(false);
        }

        public virtual bool IsAvailableToGet => gameObject.activeSelf;
    }
}