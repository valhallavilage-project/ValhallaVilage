using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace CrossProject.Ui.Core
{
    public class HudButton<THudButtonModel> : HudElementView<THudButtonModel> 
        where THudButtonModel : HudButtonModel
    {
        [SerializeField] protected Button button;

        protected override void OnBind()
        {
            button.onClick.RemoveAllListeners();
            button.OnClickAsAsyncEnumerable().ForEachAsync(_ => Model.Click(), gameObject.GetCancellationTokenOnDestroy()).Forget();
        }
    }
}
