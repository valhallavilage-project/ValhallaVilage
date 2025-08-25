using UnityEngine;

namespace L2Farm.Features.Buildings
{
    public class Building : MonoBehaviour
    {
        [SerializeField] private bool isReady;

        public bool IsReady => isReady;
    }
}
