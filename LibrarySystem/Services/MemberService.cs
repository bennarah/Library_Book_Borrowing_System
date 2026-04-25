using LibrarySystem.DTOs;
using LibrarySystem.Models;
using LibrarySystem.Repositories;

namespace LibrarySystem.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _repository;

        public MemberService(IMemberRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<MemberResponseDto>> GetAllAsync()
        {
            var members = await _repository.GetAllAsync();
            return members.Select(MapToResponse).ToList();
        }

        public async Task<MemberResponseDto?> GetByIdAsync(int id)
        {
            var member = await _repository.GetByIdAsync(id);
            return member == null ? null : MapToResponse(member);
        }

        public async Task<MemberResponseDto> CreateAsync(MemberRequestDto dto)
        {
            var member = new Member
            {
                FullName = dto.FullName,
                Email = dto.Email,
                MembershipDate = DateTime.UtcNow
            };

            var created = await _repository.CreateAsync(member);
            return MapToResponse(created);
        }

        public async Task<MemberResponseDto?> UpdateAsync(int id, MemberRequestDto dto)
        {
            var updated = new Member
            {
                FullName = dto.FullName,
                Email = dto.Email
            };

            var result = await _repository.UpdateAsync(id, updated);
            return result == null ? null : MapToResponse(result);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        private static MemberResponseDto MapToResponse(Member member)
        {
            return new MemberResponseDto
            {
                Id = member.Id,
                FullName = member.FullName,
                Email = member.Email,
                MembershipDate = member.MembershipDate
            };
        }
    }
}
