using System.Collections.Generic;
using CrossProject.Core.SaveLoad;

namespace CrossProject.Core.Quests
{
    [System.Serializable]
    public class QuestsLogPart : IGameStatePart
    {
        public Dictionary<QuestId, int> launchedQuests = new();
        public HashSet<QuestId> wonQuests = new();
        public HashSet<QuestId> lostQuests = new();
    }
}
