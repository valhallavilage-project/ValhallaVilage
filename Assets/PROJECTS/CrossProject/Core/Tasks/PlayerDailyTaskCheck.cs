using System;
using UnityEngine;

namespace CrossProject.Core
{
    public class PlayerDailyTaskCheck : MonoBehaviour
    {
        public event Action<string> OnHitDailyMaster;

        private void OnTriggerEnter(Collider other)
        {
            DailyTaskCheck dailyTaskCheck = other.GetComponent<DailyTaskCheck>();
            if(dailyTaskCheck)
            {
                OnHitDailyMaster?.Invoke(dailyTaskCheck.Name);
            }
        }
    }
}
