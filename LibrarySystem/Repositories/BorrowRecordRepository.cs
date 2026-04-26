using LibrarySystem.Data;
using LibrarySystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Repositories
{
    public class BorrowRecordRepository : IBorrowRecordRepository
    {
        private readonly AppDbContext _context;

        public BorrowRecordRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<BorrowRecord>> GetAllAsync()
        {
            return await _context.BorrowRecords
                .Include(br => br.Book)
                .Include(br => br.Member)
                .ToListAsync();
        }

        public async Task<BorrowRecord?> GetByIdAsync(int id)
        {
            return await _context.BorrowRecords
                .Include(br => br.Book)
                .Include(br => br.Member)
                .FirstOrDefaultAsync(br => br.Id == id);
        }

        public async Task<BorrowRecord> CreateAsync(BorrowRecord borrowRecord)
        {
            _context.BorrowRecords.Add(borrowRecord);
            await _context.SaveChangesAsync();
            return borrowRecord;
        }

        public async Task<BorrowRecord?> UpdateAsync(int id, BorrowRecord updated)
        {
            var borrowRecord = await _context.BorrowRecords.FindAsync(id);
            if (borrowRecord == null) return null;

            borrowRecord.MemberId = updated.MemberId;
            borrowRecord.BookId = updated.BookId;
            borrowRecord.BorrowDate = updated.BorrowDate;
            borrowRecord.ReturnDate = updated.ReturnDate;
            borrowRecord.Status = updated.Status;

            await _context.SaveChangesAsync();
            return borrowRecord;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var borrowRecord = await _context.BorrowRecords.FindAsync(id);
            if (borrowRecord == null) return false;

            _context.BorrowRecords.Remove(borrowRecord);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
