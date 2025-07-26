using Cysharp.Threading.Tasks;

namespace VContainer.Unity
{
    public interface IInitializable
    {
        UniTask Initialize();
    }
}
