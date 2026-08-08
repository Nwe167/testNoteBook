using Microsoft.AspNetCore.Mvc;
using NotesApplication.API.DTOs;
using NotesApplication.API.Models;
using NotesApplication.API.Repositories;
using NotesApplication.API.Repositories.Interfaces;

namespace NotesApplication.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly INoteRepository _noteRepository;

        public NotesController(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        // GET: api/notes?userId=1
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int userId)
        {
            var notes = await _noteRepository.GetAllAsync(userId);
            return Ok(notes.Select(n => new
            {
                id = n.NoteId,
                userId = n.UserId,
                title = n.Title,
                content = n.Content,
                createdAt = n.CreatedAt,
                updatedAt = n.UpdatedAt
            }));
        }

        // GET: api/notes/5?userId=1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, [FromQuery] int userId)
        {
            var note = await _noteRepository.GetByIdAsync(id, userId);

            if (note == null)
                return NotFound();

            return Ok(new
            {
                id = note.NoteId,
                userId = note.UserId,
                title = note.Title,
                content = note.Content,
                createdAt = note.CreatedAt,
                updatedAt = note.UpdatedAt
            });
        }

        // POST: api/notes
        [HttpPost]
        public async Task<IActionResult> Create(CreateNoteDto dto)
        {
            var note = new Note
            {
                UserId = dto.UserId,
                Title = dto.Title,
                Content = dto.Content
            };

            var newId = await _noteRepository.CreateAsync(note);
            var created = await _noteRepository.GetByIdAsync(newId, dto.UserId);

            return Ok(new
            {
                id = created!.NoteId,
                userId = created.UserId,
                title = created.Title,
                content = created.Content,
                createdAt = created.CreatedAt,
                updatedAt = created.UpdatedAt
            });
        }

        // PUT: api/notes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateNoteDto dto)
        {
            var note = new Note
            {
                NoteId = id,
                UserId = dto.UserId,
                Title = dto.Title,
                Content = dto.Content
            };

            var result = await _noteRepository.UpdateAsync(note);

            if (!result)
                return NotFound();

            var updated = await _noteRepository.GetByIdAsync(id, dto.UserId);

            return Ok(new
            {
                id = updated!.NoteId,
                userId = updated.UserId,
                title = updated.Title,
                content = updated.Content,
                createdAt = updated.CreatedAt,
                updatedAt = updated.UpdatedAt
            });
        }

        // DELETE: api/notes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] int userId)
        {
            var result = await _noteRepository.DeleteAsync(id, userId);

            if (!result)
                return NotFound();

            return Ok(new { message = "Note deleted successfully." });
        }
    }
}