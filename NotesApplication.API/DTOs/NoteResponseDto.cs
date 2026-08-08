using System;

namespace NotesApplication.API.DTOs
{
    public class NoteResponseDto
    {
        public int NoteId { get; set; }

        public int UserId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}