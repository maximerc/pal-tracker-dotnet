
using static Steeltoe.Common.HealthChecks.HealthStatus;
using Steeltoe.Common.HealthChecks;
using System.Linq;

namespace PalTracker
{
    public class TimeEntryHealthContributor : IHealthContributor
    {
        public const int MaxTimeEntries = 5;
        public string Id => "timeEntry";

        private readonly ITimeEntryRepository _timeEntryRepository;

        public TimeEntryHealthContributor(ITimeEntryRepository timeEntryRepository)
        {
            _timeEntryRepository = timeEntryRepository;
        }
        public HealthCheckResult Health()
        {
            var count = _timeEntryRepository.List().Count();

            var status = count < MaxTimeEntries ? UP : DOWN;

            var health = new HealthCheckResult { Status = status};

            health.Details.Add("count", count);
            health.Details.Add("threshold", MaxTimeEntries);
            health.Details.Add("status", status.ToString());

            return health;
        }
    }
}