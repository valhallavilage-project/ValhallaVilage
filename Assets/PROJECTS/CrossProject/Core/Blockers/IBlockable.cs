namespace CrossProject.Core
{
    public interface IBlockable
    {
        public void RequestBlock(object blockRequester);
        public void ReleaseBlock(object blockRequester);
        public bool IsBlocked { get; }
    }
}