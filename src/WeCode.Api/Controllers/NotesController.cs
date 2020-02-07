using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeCode.Api.Data;
using WeCode.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace WeCode.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/notes")]
    public class NotesController : ControllerBase
    {
        private readonly NotesContext _db;

        public NotesController(NotesContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Note>>> GetNotes()
        {
            var result = await _db.Notes.ToListAsync();
            
            return Ok(result);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Note>> GetNoteById(int id)
        {
            var result = await _db.Notes.SingleOrDefaultAsync(n => n.Id == id);
         
            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Note>> CreateNote(Note note)
        {
            await _db.Notes.AddAsync(note);
            var result = await _db.SaveChangesAsync();

            return Created($"api/notes/{note.Id}", note);
        }

    }
}
