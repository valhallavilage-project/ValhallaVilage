using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace CrossProject.Core.Cheats
{
    public class BlockableCheatPanelReaction : IInitializable
    {
        private readonly IEnumerable<IBlockable> _blockables;

        public BlockableCheatPanelReaction(
            IEnumerable<IBlockable> blockables)
        {
            _blockables = blockables;
        }

        public async UniTask Initialize()
        {
            foreach (var blockable in _blockables)
            {
                SRDebug.Instance.PanelVisibilityChanged += visible =>
                {
                    if (visible)
                        blockable.AddBlock(this);
                    else
                        blockable.RemoveBlock(this);
                };
            }
        }
    }
}