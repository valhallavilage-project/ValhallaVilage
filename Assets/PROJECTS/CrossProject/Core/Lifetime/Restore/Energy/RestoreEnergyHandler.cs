namespace CrossProject.Core
{
    public interface IRestoreEnergyHandler : IRestoreHandler
    {
    }

    public class RestoreEnergyHandler : BaseRestoreHandler, IRestoreEnergyHandler
    {
        public RestoreEnergyHandler(EnergyRestorationConfig restorationConfig, IEnergyHandler parameterHandler, ITimeService timeService)
            : base(restorationConfig, parameterHandler, timeService)
        {
        }
    }
}
