using CrossProject.Ui.Core;
using L2Farm.Scripts.Conditions;
using UnityEngine;

namespace L2Farm.Features.SimpleMonolog
{
    public class SimpleMonologPopupModel : PopupModel
    {
        public Sprite portrait;
        public string personName;
        public string message;
        public HasEnoughResourcesConditionConfig config;
    }
}
