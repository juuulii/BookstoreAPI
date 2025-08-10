using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IBookRepository
    {
        IEnumerable<Book> GetAll(bool includeDeleted = false);
        Book GetById(int id);
        void Add(Book book);
        void Update(Book book);
        void Delete(int id); // baja lógica
    }
}

