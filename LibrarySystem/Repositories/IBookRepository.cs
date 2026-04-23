using LibrarySystem.Models;

namespace LibrarySystem.Repositories
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllAsync();
        Task<Book?> GetByIdAsync(int id);
        Task<Book> CreateAsync(Book book);
        Task<Book?> UpdateAsync(int id, Book updated);
        Task<bool> DeleteAsync(int id);
    }
}
