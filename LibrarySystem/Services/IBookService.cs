using LibrarySystem.DTOs;

namespace LibrarySystem.Services
{
    public interface IBookService
    {
        Task<List<BookResponseDto>> GetAllAsync();
        Task<BookResponseDto?> GetByIdAsync(int id);
        Task<BookResponseDto> CreateAsync(BookRequestDto dto);
        Task<BookResponseDto?> UpdateAsync(int id, BookRequestDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
