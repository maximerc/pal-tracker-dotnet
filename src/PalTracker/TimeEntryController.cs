using System;
using Microsoft.AspNetCore.Mvc;

namespace PalTracker
{
  [Route("time-entries")]
    public class TimeEntryController : ControllerBase
    {
        ITimeEntryRepository _repository;
        IOperationCounter<TimeEntry> _operationCounter;

        public TimeEntryController(ITimeEntryRepository repository, IOperationCounter<TimeEntry> operationCounter)
        {
            _repository = repository;
            _operationCounter = operationCounter;
        }

         [HttpGet("{id}", Name = "GetTimeEntry")]
        public IActionResult Read(int id)
        {
            _operationCounter.Increment(TrackedOperation.Read);
            if (_repository.Contains(id))
            {
                return Ok(_repository.Find(id));
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Create([FromBody] TimeEntry toCreate)
        {
            _operationCounter.Increment(TrackedOperation.Create);
            var entry = _repository.Create(toCreate);
            return CreatedAtRoute("GetTimeEntry", new {id = entry.Id}, entry);
        }

        [HttpGet]
        public IActionResult List()
        {
            _operationCounter.Increment(TrackedOperation.List);
            return Ok(_repository.List());
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] TimeEntry theUpdate)
        {
            _operationCounter.Increment(TrackedOperation.Update);
            if (_repository.Contains(id))
            {
                return Ok(_repository.Update(id, theUpdate));
            }

            return NotFound();
        }


         [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            _operationCounter.Increment(TrackedOperation.Delete);

            if (!_repository.Contains(id))
            {
                return NotFound();
            }

            _repository.Delete(id);

            return NoContent();
        }
    }
}