using LibrarySystem.DTOs;
using LibrarySystem.Models;
using LibrarySystem.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace LibrarySystem.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repository;
        private readonly IMemoryCache _cache;

        private const string AllBooksCacheKey = "all_books";
        private const string BookCacheKeyPrefix = "book_";

        public BookService(IBookRepository repository, IMemoryCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<List<BookResponseDto>> GetAllAsync()
        {
            if (_cache.TryGetValue(AllBooksCacheKey, out List<BookResponseDto>? cached) && cached != null)
                return cached;

            var books = await _repository.GetAllAsync();
            var result = books.Select(MapToResponse).ToList();

            _cache.Set(AllBooksCacheKey, result, TimeSpan.FromMinutes(5));
            return result;
        }

        public async Task<BookResponseDto?> GetByIdAsync(int id)
        {
            var cacheKey = $"{BookCacheKeyPrefix}{id}";

            if (_cache.TryGetValue(cacheKey, out BookResponseDto? cached) && cached != null)
                return cached;

            var book = await _repository.GetByIdAsync(id);
            if (book == null) return null;

            var result = MapToResponse(book);
            _cache.Set(cacheKey, result, TimeSpan.FromMinutes(5));
            return result;
        }

        public async Task<BookResponseDto> CreateAsync(BookRequestDto dto)
        {
            if (dto.AvailableCopies > dto.TotalCopies)
                throw new InvalidOperationException("AvailableCopies cannot exceed TotalCopies.");

            var book = new Book
            {
                Title = dto.Title,
                Author = dto.Author,
                ISBN = dto.ISBN,
                TotalCopies = dto.TotalCopies,
                AvailableCopies = dto.AvailableCopies
            };

            var created = await _repository.CreateAsync(book);
            InvalidateCache(created.Id);
            return MapToResponse(created);
        }

        public async Task<BookResponseDto?> UpdateAsync(int id, BookRequestDto dto)
        {
            if (dto.AvailableCopies > dto.TotalCopies)
                throw new InvalidOperationException("AvailableCopies cannot exceed TotalCopies.");

            var updated = new Book
            {
                Title = dto.Title,
                Author = dto.Author,
                ISBN = dto.ISBN,
                TotalCopies = dto.TotalCopies,
                AvailableCopies = dto.AvailableCopies
            };

            var result = await _repository.UpdateAsync(id, updated);
            if (result == null) return null;

            InvalidateCache(id);
            return MapToResponse(result);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deleted = await _repository.DeleteAsync(id);
            if (deleted) InvalidateCache(id);
            return deleted;
        }

        private void InvalidateCache(int id)
        {
            _cache.Remove(AllBooksCacheKey);
            _cache.Remove($"{BookCacheKeyPrefix}{id}");
        }

        private static BookResponseDto MapToResponse(Book book) => new BookResponseDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            ISBN = book.ISBN,
            TotalCopies = book.TotalCopies,
            AvailableCopies = book.AvailableCopies
        };
    }
}
