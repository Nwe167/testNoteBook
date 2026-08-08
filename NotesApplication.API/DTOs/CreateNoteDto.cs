namespace NotesApplication.API.DTOs
{
    public class CreateNoteDto
    {
        public int UserId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Content { get; set; }
    }
}