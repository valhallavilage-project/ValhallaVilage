using CrossProject.Ui.Core;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace L2Farm.Features.SimpleMonolog
{
    public class SimpleMonologPopup : PopupView<SimpleMonologPopupModel>
    {
        [SerializeField] private Image _portrait;
        [SerializeField] private TMP_Text _personName;
        [SerializeField] private TMP_Text _message;
        [SerializeField] private ItemRequirement _itemRequirementPrefab;
        [SerializeField] private Transform _rootForItemRequirements;
        [SerializeField] private Button _nextButton;

        protected override void OnBind()
        {
            _portrait.sprite = Model.portrait;
            _personName.text = Model.personName;
            _message.text = Model.message;
            _message.maxVisibleCharacters = 0;
            var totalChars = _message.text.Length;
            
            DOTween.To(() => _message.maxVisibleCharacters, x => _message.maxVisibleCharacters = x, totalChars, 0.5f)
                .SetEase(Ease.Linear);
           
            _nextButton.SetUniqueCallback(Model.next);

            if (Model.resourcesData is { Count: > 0 })
            {
                _rootForItemRequirements.RemoveAllChildren();

                foreach (var data in Model.resourcesData)
                {
                    var instance = Instantiate(_itemRequirementPrefab, _rootForItemRequirements);
                    instance.Setup(data);
                }
            }
        }
    }
}
