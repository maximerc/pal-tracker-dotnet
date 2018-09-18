using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PalTracker
{
    public class MySqlTimeEntryRepository : ITimeEntryRepository
    {
        public TimeEntryContext _timeEntryContext { get; set; }
        public MySqlTimeEntryRepository(TimeEntryContext context)
        {
            _timeEntryContext = context;
        }
        public bool Contains(long id)
        {
            return _timeEntryContext.TimeEntryRecords.AsNoTracking().Any(t => t.Id == id);
        }

        public TimeEntry Create(TimeEntry timeEntry)
        {
            var timeEntryRecord = timeEntry.ToRecord();

            _timeEntryContext.TimeEntryRecords.Add(timeEntryRecord);
            _timeEntryContext.SaveChanges();

            return Find(timeEntryRecord.Id.Value);
        }

        public void Delete(long id)
        {
            _timeEntryContext.TimeEntryRecords.Remove(new TimeEntryRecord() { Id = id});
            _timeEntryContext.SaveChanges();
        }

        public TimeEntry Find(long id)
        {
            var timeEntryRecord = FindRecord(id);
            return timeEntryRecord.ToEntity();
        }

        public IEnumerable<TimeEntry> List() =>
            _timeEntryContext.TimeEntryRecords.AsNoTracking().Select(t => t.ToEntity());

        public TimeEntry Update(long id, TimeEntry timeEntry)
        {
            var recordToUpdate = timeEntry.ToRecord();
            recordToUpdate.Id = id;

            _timeEntryContext.Update(recordToUpdate);
            _timeEntryContext.SaveChanges();

            return Find(id);
        }

        private TimeEntryRecord FindRecord(long id) =>
            _timeEntryContext.TimeEntryRecords.AsNoTracking().Single(t => t.Id == id);
    }
}