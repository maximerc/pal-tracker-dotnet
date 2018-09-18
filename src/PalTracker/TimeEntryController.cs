using System;
using Microsoft.AspNetCore.Mvc;

namespace PalTracker
{
  [Route("time-entries")]
    public class TimeEntryController : ControllerBase
    {
        ITimeEntryRepository _repository;

        public TimeEntryController(ITimeEntryRepository repository)
        {
            _repository = repository;
        }

         [HttpGet("{id}", Name = "GetTimeEntry")]
        public IActionResult Read(int id)
        {
            if (_repository.Contains(id))
            {
                return Ok(_repository.Find(id));
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Create([FromBody] TimeEntry toCreate)
        {
            var entry = _repository.Create(toCreate);
            return CreatedAtRoute("GetTimeEntry", new {id = entry.Id}, entry);
        }

        [HttpGet]
        public IActionResult List()
        {
            return Ok(_repository.List());
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] TimeEntry theUpdate)
        {
            if (_repository.Contains(id))
            {
                return Ok(_repository.Update(id, theUpdate));
            }

            return NotFound();
        }


         [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            if (!_repository.Contains(id))
            {
                return NotFound();
            }

            _repository.Delete(id);

            return NoContent();
        }
    }
}