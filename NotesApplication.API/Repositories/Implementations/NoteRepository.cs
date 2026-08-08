using Dapper;
using NotesApplication.API.Data;
using NotesApplication.API.Models;
using NotesApplication.API.Repositories.Interfaces;

namespace NotesApplication.API.Repositories.Implementations
{
    public class NoteRepository : INoteRepository
    {
        private readonly DapperContext _context;

        public NoteRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Note>> GetAllAsync(int userId)
        {
            var sql = @"SELECT *
                        FROM Notes
                        WHERE UserId=@UserId
                        ORDER BY CreatedAt DESC";

            using var connection = _context.CreateConnection();

            return await connection.QueryAsync<Note>(sql, new { UserId = userId });
        }

        public async Task<Note?> GetByIdAsync(int id, int userId)
        {
            var sql = @"SELECT *
                        FROM Notes
                        WHERE NoteId=@Id AND UserId=@UserId";

            using var connection = _context.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<Note>(sql, new { Id = id, UserId = userId });
        }

        public async Task<int> CreateAsync(Note note)
        {
            var sql = @"
                INSERT INTO Notes
                (
                    UserId,
                    Title,
                    Content
                )
                VALUES
                (
                    @UserId,
                    @Title,
                    @Content
                );

                SELECT CAST(SCOPE_IDENTITY() as int);
            ";

            using var connection = _context.CreateConnection();

            return await connection.ExecuteScalarAsync<int>(sql, note);
        }

        public async Task<bool> UpdateAsync(Note note)
        {
            var sql = @"
                UPDATE Notes
                SET
                    Title=@Title,
                    Content=@Content,
                    UpdatedAt=CASE
                        WHEN Title != @Title OR ISNULL(Content,'') != ISNULL(@Content,'')
                        THEN GETDATE()
                        ELSE UpdatedAt
                    END
                WHERE
                    NoteId=@NoteId";

            using var connection = _context.CreateConnection();

            return await connection.ExecuteAsync(sql, note) > 0;
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var sql = @"DELETE FROM Notes
                        WHERE NoteId=@Id AND UserId=@UserId";

            using var connection = _context.CreateConnection();

            return await connection.ExecuteAsync(sql, new { Id = id, UserId = userId }) > 0;
        }
    }
}