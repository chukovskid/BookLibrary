using BookLibrary.Models;
using BookLibrary.ViewModels;
using PDFUpload.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDFUpload.Repos
{
    public interface ILibraryRepository
    {

        void Add<T>(T entity) where T : class;
        void Delete<T>(T entit) where T : class;
        Task<bool> SaveAll();
        Task<IEnumerable<Book>> GetBooks();
        Task<Book> GetBook(int id);
        Task<Tag> GetTag(int id);
        Task<IEnumerable<Tag>> GetTags(string query);
        Task<Book> GetNewBook();
        Task<IEnumerable<Tag>> TagsForNewBook();
        Task<IEnumerable<BookTags>> FoundBookTags(List<int> SelectedTags);
        Task<IEnumerable<Book>> FoundBooks(IEnumerable<ViewTagModel> bookTags);







    }
}
