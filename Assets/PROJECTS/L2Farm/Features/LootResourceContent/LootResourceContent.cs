using CrossProject.Core.Content;
using CrossProject.Core.PROJECTS.CrossProject.Core.InGameResources;
using Sirenix.OdinInspector;
using UnityEngine;

namespace L2Farm.Features.LootResourceContent
{
    public class LootResourceContent : IResourceContent
    {
        [SerializeField]
        private ResourceId resourceId;

        [SerializeField]
        private bool randomAmount;

        [SerializeField]
        [HideIf("$randomAmount")]
        private int amount = 1;

        [SerializeField]
        [ShowIf("$randomAmount")]
        private int lowerRange = 1;

        [SerializeField]
        [ShowIf("$randomAmount")]
        private int upperRange = 10;

        public ResourceId Resource
        {
            get => resourceId;
            set => resourceId = value;
        }

        public int Amount
        {
            get => randomAmount
                ? Random.Range(lowerRange, upperRange + 1)
                : amount;
            set => amount = value;
        }
    }
}
