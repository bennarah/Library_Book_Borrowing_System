using LibrarySystem.DTOs;

namespace LibrarySystem.Services
{
    public interface IMemberService
    {
        Task<List<MemberResponseDto>> GetAllAsync();
        Task<MemberResponseDto?> GetByIdAsync(int id);
        Task<MemberResponseDto> CreateAsync(MemberRequestDto dto);
        Task<MemberResponseDto?> UpdateAsync(int id, MemberRequestDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
