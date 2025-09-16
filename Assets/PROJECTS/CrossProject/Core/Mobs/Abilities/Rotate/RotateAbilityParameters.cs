namespace CrossProject.Core
{
    public struct RotateAbilityParameters
    {
        public float RotationSpeed { get; }
        public float RotationDamper { get; }

        public RotateAbilityParameters(float rotationSpeed, float rotationDamper)
        {
            RotationSpeed = rotationSpeed;
            RotationDamper = rotationDamper;
        }
    }
}