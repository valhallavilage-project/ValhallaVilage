using CrossProject.Core.Quests;
using CrossProject.Ui.Core;

namespace L2Farm.Features
{
    public class ActiveQuestsModel : ScreenModel
    {
        public QuestsLogPart GameStatePart { get; set; }
        public QuestService QuestService { get; set; }
    }
}
