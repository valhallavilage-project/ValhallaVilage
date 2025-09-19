namespace CrossProject.Core
{
    public interface IRestoreHealthHandler : IRestoreHandler
    {
    }

    public class RestoreHealthHandler : BaseRestoreHandler, IRestoreHealthHandler
    {
        public RestoreHealthHandler(HealthRestorationConfig restorationConfig, IHealthHandler parameterHandler, ITimeService timeService)
            : base(restorationConfig, parameterHandler, timeService)
        {
        }
    }
}
