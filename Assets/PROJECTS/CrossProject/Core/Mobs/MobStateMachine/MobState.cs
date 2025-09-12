namespace CrossProject.Core
{
    public enum MobState
    {
        Idle = 0,
        Roam = 1,
        Notice = 2,
        RotateToTarget = 3,
        ApproachTarget = 4,
        ReturnToRoamArea = 5,
        Attack = 6,
        AttackPause = 7,
        RoamRotation = 8,
        WaitForTarget = 9,
        Die = 10
    }
}
