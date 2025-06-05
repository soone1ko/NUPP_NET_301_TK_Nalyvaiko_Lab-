using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibrarySystem.Common;
using LibrarySystem.Infrastructure.Data;
using LibrarySystem.REST.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.REST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JournalsController : ControllerBase
    {
        private readonly ICrudServiceAsync<Journal> _journalService;

        public JournalsController(ICrudServiceAsync<Journal> journalService)
        {
            _journalService = journalService;
        }

        // GET: api/Journals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JournalDto>>> GetAll()
        {
            var journals = await _journalService.ReadAllAsync();
            var result = new List<JournalDto>();
            foreach (var j in journals)
            {
                result.Add(new JournalDto
                {
                    Id = j.Id,
                    Name = j.Name,
                    Volume = j.Volume,
                    Issue = j.Issue,
                    Publisher = j.Publisher
                });
            }
            return Ok(result);
        }

        // GET: api/Journals/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<JournalDto>> GetById(Guid id)
        {
            var journal = await _journalService.ReadAsync(id);
            if (journal == null)
                return NotFound();

            var dto = new JournalDto
            {
                Id = journal.Id,
                Name = journal.Name,
                Volume = journal.Volume,
                Issue = journal.Issue,
                Publisher = journal.Publisher
            };
            return Ok(dto);
        }

        // POST: api/Journals
        [HttpPost]
        public async Task<ActionResult<JournalDto>> Create([FromBody] JournalDto dto)
        {
            var newJournal = new Journal
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Volume = dto.Volume,
                Issue = dto.Issue,
                Publisher = dto.Publisher
            };

            await _journalService.CreateAsync(newJournal);

            var createdDto = new JournalDto
            {
                Id = newJournal.Id,
                Name = newJournal.Name,
                Volume = newJournal.Volume,
                Issue = newJournal.Issue,
                Publisher = newJournal.Publisher
            };

            return CreatedAtAction(nameof(GetById), new { id = createdDto.Id }, createdDto);
        }

        // PUT: api/Journals/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] JournalDto dto)
        {
            var existing = await _journalService.ReadAsync(id);
            if (existing == null)
                return NotFound();

            existing.Name = dto.Name;
            existing.Volume = dto.Volume;
            existing.Issue = dto.Issue;
            existing.Publisher = dto.Publisher;

            await _journalService.UpdateAsync(existing);

            return NoContent();
        }

        // DELETE: api/Journals/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existing = await _journalService.ReadAsync(id);
            if (existing == null)
                return NotFound();

            await _journalService.RemoveAsync(existing);

            return NoContent();
        }
    }
}
