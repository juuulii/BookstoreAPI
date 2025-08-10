using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class BookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public IEnumerable<Book> GetAll(bool includeDeleted = false)
        {
            return _bookRepository.GetAll(includeDeleted);
        }

        public Book GetById(int id)
        {
            return _bookRepository.GetById(id);
        }

        public void Add(Book book)
        {
            _bookRepository.Add(book);
        }

        public void Update(Book book)
        {
            _bookRepository.Update(book);
        }

        public void Delete(int id)
        {
            _bookRepository.Delete(id);
        }
    }
}


