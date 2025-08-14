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
            //close button
            if (Model.config != null)
            {
                //next button
                rootForConditionItems.GetComponentsInChildren<MonologConditionItem>();
            }
        }
    }
}
