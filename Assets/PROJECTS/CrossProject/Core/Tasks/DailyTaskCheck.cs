using UnityEngine;

namespace CrossProject.Core
{
    public class DailyTaskCheck : MonoBehaviour
    {
        [SerializeField] private string _name;

        public string Name => _name;
    }
}
