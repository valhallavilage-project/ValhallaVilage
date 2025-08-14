using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CrossProject.Core.Quests
{
    [CreateAssetMenu(menuName = "Cross Project/QuestSetConfig", fileName = "QuestSetConfig")]
    public class QuestSetConfig : ScriptableObject
    {
        [SerializeReference]
        public List<QuestConfig> items = new();

        public QuestConfig Get(QuestId id) => items.FirstOrDefault(x => new QuestId(x.id) == id);
    }
}
