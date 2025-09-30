using CrossProject.Extensions;
using CrossProject.Ui.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace L2Farm.Features
{
    public class ConfirmPopup : PopupView<ConfirmPopupModel>
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Button _confirmButton;
        [SerializeField] private Button _backgroundButton;

        protected override void OnBind()
        {
            _confirmButton.SetUniqueCallback(Model.Close);
            _backgroundButton.SetUniqueCallback(Model.Close);

            _text.text = Model.Text;
        }
    }
}
