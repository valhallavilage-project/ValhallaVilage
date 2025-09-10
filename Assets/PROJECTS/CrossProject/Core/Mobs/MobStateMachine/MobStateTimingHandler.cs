namespace CrossProject.Core
{
    public interface IMobStateTimingHandler : IStateTimingHandler<MobState>
    {
    }

    public class MobStateTimingHandler : BaseStateTimingHandler<MobState>, IMobStateTimingHandler
    {
    }
}
