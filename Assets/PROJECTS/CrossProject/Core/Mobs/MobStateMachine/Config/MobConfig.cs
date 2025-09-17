using UnityEngine;

namespace CrossProject.Core
{
    [CreateAssetMenu(fileName = "MobConfig", menuName = "ScriptableObjects/Mob/MobConfig", order = 0)]
    public class MobConfig : ScriptableObject
    {
        [Header("Movement")]
        [SerializeField] private float _maxSpeed = 1;
        [SerializeField] private float _acceleration = 20;
        [SerializeField] private float _maxAcceleration = 100;
        [SerializeField] private float _rotationSpeed = 100;
        [SerializeField] private float _rotationDamper = 0.5f;
        [SerializeField] private float _torqueStrength = 1f;
        [SerializeField] private float _minDistanceToApproach = 5f;

        [Header("Roam")]
        [SerializeField] private float _roamingMaxSpeed = 1;
        [SerializeField] private float _roamingMinPathLength = 2;
        [SerializeField] private float _roamingMinAngleBeforeForceRotate = 2;

        [Header("Attack")]
        [SerializeField] private float _attackDistance = 1;
        [SerializeField] private float _attackDamage = 1;

        [Header("Heath")]
        [SerializeField] private float _health = 50;
        
        [Header("Lifetime")]
        [SerializeField] private float _corpseDecayTime = 5;
        
        [Header("Experience")]
        [SerializeField] private float _experienceReward = 15;
        
        public float MaxSpeed => _maxSpeed;
        public float Acceleration => _acceleration;
        public float MaxAcceleration => _maxAcceleration;
        public float RotationSpeed => _rotationSpeed;
        public float RotationDamper => _rotationDamper;
        public float RoamingMaxSpeed => _roamingMaxSpeed;
        public float AttackDistance => _attackDistance;
        public float Health => _health;
        public float MinDistanceToApproach => _minDistanceToApproach;
        public float RoamingMinPathLength => _roamingMinPathLength;
        public float RoamingMinAngleBeforeForceRotate => _roamingMinAngleBeforeForceRotate;
        public float AttackDamage => _attackDamage;
        public float TorqueStrength => _torqueStrength;
        public float CorpseDecayTime => _corpseDecayTime;
        public float ExperienceReward => _experienceReward;
    }
}