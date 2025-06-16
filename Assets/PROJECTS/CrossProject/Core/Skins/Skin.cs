using UnityEngine;

namespace CrossProject.Core.Skins
{
    public class Skin : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        public Animator Animator => animator;
    }
}