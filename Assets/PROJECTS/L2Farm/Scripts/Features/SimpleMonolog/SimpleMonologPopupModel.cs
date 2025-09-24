using System.Collections.Generic;
using CrossProject.Ui.Core;
using UnityEngine;

namespace L2Farm.Features.SimpleMonolog
{
    public class SimpleMonologPopupModel : PopupModel
    {
        public Sprite portrait;
        public string personName;
        public string message;
        public List<MonologResourceData> resourcesData;
        public System.Action close;
        public System.Action next;
    }
}
