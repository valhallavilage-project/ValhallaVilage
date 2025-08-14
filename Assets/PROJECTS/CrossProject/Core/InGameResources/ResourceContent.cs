using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CrossProject.Core.InGameResources
{
    [Serializable]
    public class ResourceContent : IResourceContent
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

        public ResourceContent(ResourceId id, int amount = 0)
        {
            Resource = id;
            Amount = amount;
        }
    }
}
