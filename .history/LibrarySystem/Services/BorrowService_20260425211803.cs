using LibrarySystem.DTOs;
using LibrarySystem.Models;
using LibrarySystem.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Services
{
    public class BorrowService : IBorrowService
    {
        private readonly IBorrowRecordRepository _borrowRepo;
        private readonly IBookRepository _bookRepo;
        private readonly IMemberRepository _memberRepo;

        public BorrowService(
            IBorrowRecordRepository borrowRepo,
            IBookRepository bookRepo,
            IMemberRepository memberRepo)
        {
            _borrowRepo = borrowRepo;
            _bookRepo = bookRepo;
            _memberRepo = memberRepo;
        }

        public async Task<List<BorrowResponseDto>> GetAllAsync()
        {
            var records = await _borrowRepo.GetAllAsync();
            return records.Select(MapToResponse).ToList();
        }

        public async Task<BorrowResponseDto?> GetByIdAsync(int id)
        {
            var record = await _borrowRepo.GetByIdAsync(id);
            return record == null ? null : MapToResponse(record);
        }

        public async Task<BorrowResponseDto> BorrowAsync(BorrowRequestDto dto)
        {
            var book = await _bookRepo.GetByIdAsync(dto.BookId)
                ?? throw new InvalidOperationException($"Book with ID {dto.BookId} not found.");

            if (book.AvailableCopies < 1)
                throw new InvalidOperationException("No available copies of this book.");

            var member = await _memberRepo.GetByIdAsync(dto.MemberId)
                ?? throw new InvalidOperationException($"Member with ID {dto.MemberId} not found.");

            book.AvailableCopies--;
            book.RowVersion++;

            var record = new BorrowRecord
            {
                BookId = dto.BookId,
                MemberId = dto.MemberId,
                BorrowDate = DateTime.UtcNow,
                Status = BorrowStatus.Borrowed
            };

            try
            {

                var created = await _borrowRepo.CreateAsync(record);
                created.Book = book;
                created.Member = member;
                return MapToResponse(created);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new InvalidOperationException(
                    "Another request just borrowed the last copy. Please try again.");
            }
        }

        public async Task<BorrowResponseDto?> ReturnAsync(int id)
        {
            var record = await _borrowRepo.GetByIdAsync(id);
            if (record == null) return null;

            if (record.Status == BorrowStatus.Returned)
                throw new InvalidOperationException("This book has already been returned.");

            var book = await _bookRepo.GetByIdAsync(record.BookId)
                ?? throw new InvalidOperationException($"Book with ID {record.BookId} not found.");

            book.AvailableCopies++;
            book.RowVersion++;

            var updated = new BorrowRecord
            {
                BookId = record.BookId,
                MemberId = record.MemberId,
                BorrowDate = record.BorrowDate,
                ReturnDate = DateTime.UtcNow,
                Status = BorrowStatus.Returned
            };

            try
            {
                var result = await _borrowRepo.UpdateAsync(id, updated);
                if (result == null) return null;

                result.Book = book;
                result.Member = record.Member;
                return MapToResponse(result);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new InvalidOperationException(
                    "A conflict occurred while returning the book. Please try again.");
            }
        }

        private static BorrowResponseDto MapToResponse(BorrowRecord r) => new BorrowResponseDto
        {
            Id = r.Id,
            BookId = r.BookId,
            BookTitle = r.Book?.Title ?? string.Empty,
            MemberId = r.MemberId,
            MemberName = r.Member?.FullName ?? string.Empty,
            BorrowDate = r.BorrowDate,
            ReturnDate = r.ReturnDate,
            Status = r.Status.ToString()
        };
    }
}
