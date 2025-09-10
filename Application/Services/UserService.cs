using Application.Dtos;
using Domain.Entities;
using Domain.Interfaces;
using System.Collections.Generic;

namespace Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _repository;
        //private: Solo se puede usar dentro de la clase UserService. Nadie desde afuera puede acceder a _repository directamente

        //readonly: Significa que la variable solo se puede asignar una vez, ya sea:
        //en el momento de la declaración, o
        // en el constructor de la clase.
        // Después de eso, no la podés cambiar nunca más.
        //Esto da seguridad: garantiza que _repository siempre va a apuntar al mismo objeto durante la vida del UserService. */

        //Al usar una interfaz en vez de la clase concreta (UserRepository), lográs que tu servicio sea más flexible y testeable.


        //esto es un constructor
        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        /* ¿Qué significa todo junto?
        Básicamente:

        Cada vez que alguien cree un UserService, necesita un IUserRepository.

        Ese repositorio se guarda en _repository, y tu servicio lo puede usar en sus métodos. */
        
        public UserModel? Get(string name)
        {
            var user = _repository.Get(name); //busca un usuario en la BD.
            if (user == null) return null;

            return new UserModel() //Si lo encuentra, lo convierte en UserModel y lo devuelve al Controller.
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                Role = user.Role,
                Id = user.Id
            };
        }

        public List<User> Get(bool includeDeleted = false) //Llama a _repository.Get(includeDeleted) 
        // → trae todos los usuarios (con o sin eliminados).
        {
            return _repository.Get(includeDeleted); //Devuelve directamente la lista de User al Controller.
        }

        public UserModel? GetById(int id) //lo uso en el update, se usa solo como chequeo previo antes de actualizar.
        {
            var user = _repository.GetById(id);
            if (user == null) return null;

            return new UserModel()
            {
                Email = user.Email,
                Password = user.Password,
                Role = user.Role,
                Id = user.Id,
                Name = user.Name
            };
        }

        public void Update(int id, UserForUpdateRequest request)
        {
            var existingUser = _repository.GetById(id); //Llama a _repository.GetById(id) para traer al usuario actual.
            if (existingUser == null) return;

            existingUser.Name = request.Name; //Actualiza sus datos con los del request (Name, Email, Password, Role).
            existingUser.Email = request.Email;
            existingUser.Password = request.Password;
            existingUser.Role = request.Role;

            _repository.Update(existingUser); //Manda el usuario actualizado a _repository.Update()
        }

        public int AddUser(UserForAddRequest request) //Crea un User a partir del DTO recibido.
        {
            var user = new User()
            {
                Name = request.Name,
                Email = request.Email,
                Password = request.Password
            };

            return _repository.AddUser(user); //Llama a _repository.AddUser(user) para guardarlo en la BD.
        }

        public UserModel? CheckCredentials(CredentialsRequest credentials)
        {
            var user = _repository.Get(credentials.Name); //Busca al usuario por nombre con _repository.Get().
            if (user != null && user.Password == credentials.Password)
            {
                return new UserModel()
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role,
                    Password = string.Empty
                }; //Si coincide la contraseña, devuelve un UserModel (con Password vacío para no exponerlo).
            }
            return null;
        }

        public void Delete(int id)
        {
            _repository.Delete(id); //Llama a _repository.Delete(id) → marca al usuario como eliminado en la BD.
        }
    }
}
