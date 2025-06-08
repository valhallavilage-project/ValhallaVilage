namespace CrossProject.Core
{
    public interface IBlocksManager
    {
        public void RequestBlock(object blockRequester);
        public void ReleaseBlock(object blockRequester);
        public bool IsBlocked { get; }
    }
}