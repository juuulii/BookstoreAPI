using Application.Dtos;
using Domain.Entities;
using Domain.Interfaces;
using System.Collections.Generic;

namespace Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public UserModel Get(string name)
        {
            var user = _repository.Get(name);
            if (user == null) return null;

            return new UserModel()
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                Role = user.Role,
                Id = user.Id
            };
        }

        public List<User> Get()
        {
            return _repository.Get();
        }

        public UserModel GetById(int id)
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

        public void Update(UserModel user)
        {
            _repository.Update(new User()
            {
                Email = user.Email,
                Password = user.Password,
                Role = user.Role,
                Id = user.Id,
                Name = user.Name
            });
        }

        public int AddUser(UserForAddRequest request)
        {
            var user = new User()
            {
                Name = request.Name,
                Email = request.Email,
                Password = request.Password
            };

            return _repository.AddUser(user);
        }

        public UserModel? CheckCredentials(CredentialsRequest credentials)
        {
            var user = _repository.Get(credentials.Name);
            if (user != null && user.Password == credentials.Password)
            {
                return new UserModel()
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role,
                    Password = string.Empty
                };
            }
            return null;
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}
