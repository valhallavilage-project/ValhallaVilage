using Cysharp.Threading.Tasks;

namespace VContainer.Unity
{
    public interface IInitializable
    {
        bool IsInitialized { get; }
        UniTask Initialize();
    }
}
