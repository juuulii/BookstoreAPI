using Application.Dtos;   // BookDto
using Application.DTOs;  // CreateBookRequest  <-- ¡este es el namespace que vos usás!
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
            return _bookRepository.GetById(id); // Tu repo ya hace Include(...)
        }

        // Crear libro y devolver DTO consistente
        public BookDto CreateBook(CreateBookRequest request)
        {
            var book = new Book
            {
                Title = request.Title,
                Price = request.Price,
                Stock = request.Stock,
                AuthorId = request.AuthorId,
                PublisherId = request.PublisherId,
                CategoryId = request.CategoryId,
                IsDeleted = false
            };

            _bookRepository.Add(book);

            // Volver a cargar con relaciones (tu GetById ya incluye Author/Publisher/Category)
            var createdBook = _bookRepository.GetById(book.Id);
            if (createdBook == null) return null;

            // Mapear a DTO
            return new BookDto
            {
                Id = createdBook.Id,
                Title = createdBook.Title,
                Price = createdBook.Price,
                Stock = createdBook.Stock,
                AuthorName = createdBook.Author?.Name,
                AuthorLastName = createdBook.Author?.LastName,
                PublisherName = createdBook.Publisher?.Name,
                CategoryName = createdBook.Category?.Name
            };
        }

        public bool Update(int id, UpdateBookRequest request)
        {
            var book = _bookRepository.GetById(id);

            if (book == null || book.IsDeleted)
                return false;

            book.Title = request.Title;
            book.Price = request.Price;
            book.Stock = request.Stock;
            book.AuthorId = request.AuthorId;
            book.PublisherId = request.PublisherId;
            book.CategoryId = request.CategoryId;

            _bookRepository.Update(book);
            return true;
        }

        public bool Delete(int id)
        {
            var book = _bookRepository.GetById(id);

            if (book == null || book.IsDeleted)
                return false;

            _bookRepository.Delete(id); // baja lógica
            return true;
        }
    }
    
}




