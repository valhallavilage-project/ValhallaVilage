using CrossProject.Ui.Core;
using CrossProject.Ui.Implementations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace L2Farm.Features
{
    public class ConfirmPopup : PopupView<ConfirmPopupModel>
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Button _confirmButton;
        [SerializeField] private GameObject _yesNoButtonState;
        [SerializeField] private Button _yesButton;
        [SerializeField] private Button _noButton;
        [SerializeField] private Button _backgroundButton;

        protected override void OnBind()
        {
            _confirmButton.SetUniqueCallback(Model.Close);
            _backgroundButton.SetUniqueCallback(Model.Close);
            _yesButton.SetUniqueCallback(Model.Close);
            _noButton.SetUniqueCallback(Model.Close);
            
            _yesButton.onClick.AddListener(PositiveResult);
            _noButton.onClick.AddListener(NegativeResult);

            if (Model.Data.Buttons == ConfirmPopupButtonsType.YesNo)
            {
                _backgroundButton.onClick.AddListener(NegativeResult);
            }

            _text.text = Model.Data.Text;
            _yesNoButtonState.SetActive(Model.Data.Buttons == ConfirmPopupButtonsType.YesNo);
            _confirmButton.gameObject.SetActive(Model.Data.Buttons == ConfirmPopupButtonsType.Ok);
        }

        private void PositiveResult()
        {
            Model.Result.Value = true;
        }

        private void NegativeResult()
        {
            Model.Result.Value = false;
        }
    }
}
