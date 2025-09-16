using System;

namespace CrossProject.Core
{
    public class LocalTimeService : ITimeService
    {
        public DateTime Now => DateTime.UtcNow;
    }
}
