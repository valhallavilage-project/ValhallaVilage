using UnityEngine;

namespace CrossProject.Core
{
    public abstract class RestorationConfig : ScriptableObject
    {
        [SerializeField] private float _valueToRestoreForOneInterval = 1;
        [SerializeField] private int _intervalInSeconds = 60;

        public float ValueToRestoreForOneInterval => _valueToRestoreForOneInterval;
        public int IntervalInSeconds => _intervalInSeconds;
    }
}
