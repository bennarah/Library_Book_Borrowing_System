using LibrarySystem.Models;

namespace LibrarySystem.Repositories
{
    public interface IMemberRepository
    {
        Task<List<Member>> GetAllAsync();
        Task<Member?> GetByIdAsync(int id);
        Task<Member> CreateAsync(Member member);
        Task<Member?> UpdateAsync(int id, Member updated);
        Task<bool> DeleteAsync(int id);
    }
}
