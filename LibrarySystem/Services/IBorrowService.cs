using LibrarySystem.DTOs;

namespace LibrarySystem.Services
{
    public interface IBorrowService
    {
        Task<List<BorrowResponseDto>> GetAllAsync();
        Task<BorrowResponseDto?> GetByIdAsync(int id);
        Task<BorrowResponseDto> BorrowAsync(BorrowRequestDto dto);
        Task<BorrowResponseDto?> ReturnAsync(int id);
    }
}
