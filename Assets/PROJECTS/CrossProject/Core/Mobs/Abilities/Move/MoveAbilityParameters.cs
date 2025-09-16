namespace CrossProject.Core
{
    public struct MoveAbilityParameters
    {
        public float Acceleration { get; }
        public float MaxAcceleration { get; }

        public MoveAbilityParameters(float acceleration, float maxAcceleration)
        {
            Acceleration = acceleration;
            MaxAcceleration = maxAcceleration;
        }
    }
}