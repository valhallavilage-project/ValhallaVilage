using Cysharp.Threading.Tasks;

namespace L2Farm
{
    public interface ITimerCallback
    {
        UniTask Execute();
    }
}