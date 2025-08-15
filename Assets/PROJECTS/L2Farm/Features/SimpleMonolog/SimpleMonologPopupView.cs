using CrossProject.Ui.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace L2Farm.Features.SimpleMonolog
{
    public class SimpleMonologPopupView : PopupView<SimpleMonologPopupModel>
    {
        [SerializeField]
        private Image portrait;

        [SerializeField]
        private TMP_Text personName;

        [SerializeField]
        private TMP_Text message;

        [SerializeField]
        private MonologConditionItem monologConditionItem;

        [SerializeField]
        private Transform rootForConditionItems;

        [SerializeField]
        private Button close;

        [SerializeField]
        private Button next;

        protected override void OnBind()
        {
            portrait.sprite = Model.portrait;
            personName.text = Model.personName;
            message.text = Model.message;
            close.onClick.RemoveAllListeners();
            close.onClick.AddListener(() => Model.close?.Invoke());
            if (Model.hasEnoughResourcesConditionConfig != null && Model.hasEnoughResourcesConditionConfig.resourceConditions.Count > 0)
            {
                next.onClick.RemoveAllListeners();
                next.onClick.AddListener(() => Model.next?.Invoke());
                rootForConditionItems.GetComponentsInChildren<MonologConditionItem>();
            }
        }
    }
}
