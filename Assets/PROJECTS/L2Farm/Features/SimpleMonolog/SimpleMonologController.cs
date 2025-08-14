using CrossProject.Core.Quests;

namespace L2Farm.Features.SimpleMonolog
{
    public class SimpleMonologController
    {
        private readonly QuestService _questService;

        public SimpleMonologController(QuestService questService)
        {
            _questService = questService;
        }

        
    }
}
