using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.DTOs
{
    public class BorrowRequestDto
    {
        [Required]
        public int BookId { get; set; }

        [Required]
        public int MemberId { get; set; }
    }

    public class BorrowResponseDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public int MemberId { get; set; }
        public string MemberName { get; set; } = string.Empty;
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
