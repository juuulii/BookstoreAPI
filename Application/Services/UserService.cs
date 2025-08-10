using Application.Dtos;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            UserModel model = new UserModel()
            {
                Name = name,
                Email = user.Email,
                Password = user.Password,
                Role = user.Role,
                Id = user.Id
            };
            return model;
        }

        public List<User> Get()
        {
            return _repository.Get();
        }

        public UserModel GetById(int id)
        {
            var user = _repository.GetById(id);
            UserModel model = new()
            {
                Email = user.Email,
                Password = user.Password,
                Role = user.Role,
                Id = user.Id

            };
            return model;
        }

        public void Update(UserModel user)
        {
            _repository.Update(new User()
            {
                Email = user.Email,
                Password = user.Password,
                Role = user.Role,
                Id = user.Id
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
            UserModel? user = Get(credentials.Name);
            if (user.Password == credentials.Password)
            {
                user.Password = string.Empty;
                return user;
            }
            return null;


        }
    }
}