namespace CrossProject.Core
{
    public interface IMobAnimationHandler : IAnimationHandler<MobState, MobAnimationLayer>
    {
    }

    public class MobAnimationHandler : AnimationHandler<MobState, MobAnimationLayer>, IMobAnimationHandler
    {

    }
}
