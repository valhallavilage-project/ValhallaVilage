using CrossProject.Core.Quests;
using CrossProject.Ui.Core;

namespace L2Farm.Features
{
    public class ActiveQuestsScreenModel : ScreenModel
    {
        public QuestsLogPart GameStatePart { get; set; }
        public QuestService QuestService { get; set; }
        public IResourceConditionService ResourceConditionService { get; set; }
    }
}
