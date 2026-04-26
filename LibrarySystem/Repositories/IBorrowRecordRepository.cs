using LibrarySystem.Models;

namespace LibrarySystem.Repositories
{
    public interface IBorrowRecordRepository
    {
        Task<List<BorrowRecord>> GetAllAsync();
        Task<BorrowRecord?> GetByIdAsync(int id);
        Task<BorrowRecord> CreateAsync(BorrowRecord borrowRecord);
        Task<BorrowRecord?> UpdateAsync(int id, BorrowRecord updated);
        Task<bool> DeleteAsync(int id);
    }
}