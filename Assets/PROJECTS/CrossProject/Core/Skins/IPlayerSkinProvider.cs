using UnityEngine;

namespace CrossProject.Core.Skins
{
    public interface IPlayerSkinProvider
    {
        Transform PlayerSkinRoot { get; }
    }
}