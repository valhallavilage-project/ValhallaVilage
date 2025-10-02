using System.Collections.Generic;
using CrossProject.Extensions;
using CrossProject.Ui.Core;
using UnityEngine;
using UnityEngine.UI;

namespace L2Farm.Features
{
    public class ActiveQuestsScreen : ScreenView<ActiveQuestsModel>
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private QuestItemPool _itemPool;

        private List<ActiveQuestsItem> _questItems = new();
        
        protected override void OnBind()
        {
            _closeButton.SetUniqueCallback(Model.Close);
            var launchedQuests = Model.GameStatePart.launchedQuests;

            foreach (var questKeyValue in launchedQuests)
            {
                
            }
        }

        public void ClearItems()
        {
            foreach (var questItem in _questItems)
            {
                questItem.Clear();
            }
        }
    }
}
