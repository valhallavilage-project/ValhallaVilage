namespace CrossProject.Core
{
    public interface IBlockable
    {
        public void AddBlock(object blockRequester);
        public void RemoveBlock(object blockRequester);
        public bool IsBlocked { get; }
    }
}