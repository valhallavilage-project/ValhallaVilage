using System;

namespace CrossProject.Core
{
    public interface IBlockable
    {
        public void AddBlock(Type blockRequester);
        public void RemoveBlock(Type blockRequester);
        public bool IsBlocked { get; }
    }
}