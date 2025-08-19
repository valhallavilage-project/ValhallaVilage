using CrossProject.Extensions;
using CrossProject.Ui.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace L2Farm.Features.SimpleMonolog
{
    public class SimpleMonologPopup : PopupView<SimpleMonologPopupModel>
    {
        [SerializeField]
        private Image portrait;

        [SerializeField]
        private TMP_Text personName;

        [SerializeField]
        private TMP_Text message;

        [SerializeField]
        private ItemRequirement itemRequirement;

        [SerializeField]
        private Transform rootForItemRequirements;

        [SerializeField]
        private Button close;

        [SerializeField]
        private Button next;

        protected override void OnBind()
        {
            portrait.sprite = Model.portrait;
            personName.text = Model.personName;
            message.text = Model.message;
            close.SetUniqueCallback(Model.close);
            next.SetUniqueCallback(Model.next);

            if (Model.resourcesData is { Count: > 0 })
            {
                rootForItemRequirements.RemoveAllChildren();
                foreach (var data in Model.resourcesData)
                {
                    var instance = Instantiate(itemRequirement, rootForItemRequirements);
                    instance.SetVisuals(data);
                }
            }
        }
    }
}
