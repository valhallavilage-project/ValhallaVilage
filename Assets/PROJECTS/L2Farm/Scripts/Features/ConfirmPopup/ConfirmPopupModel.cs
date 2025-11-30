using CrossProject.Ui.Core;
using CrossProject.Ui.Implementations;
using Cysharp.Threading.Tasks;

namespace L2Farm.Features
{
    public class ConfirmPopupModel : PopupModel
    {
        public AsyncReactiveProperty<bool> Result { get; } = new AsyncReactiveProperty<bool>(false);
        
        public ConfirmPopupData Data { get; set; }
    }
}
