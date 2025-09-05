using Domain.Entities;
using Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

//el contexto se hace en las interfaces?

namespace Infraestructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _context;
        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }

        public User? Get(string name)
        {
            return _context.Users.FirstOrDefault(u => u.Name == name && !u.IsDeleted);
        }

        public List<User> Get()
        {
            return _context.Users.Where(u => !u.IsDeleted).ToList();
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public User GetById(int id)
        {
            return _context.Users.First(u => u.Id == id && !u.IsDeleted);
        }

        public int AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user.Id;
        }

        public void Delete(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                user.IsDeleted = true;
                _context.Users.Update(user);
                _context.SaveChanges();
            }
        }
    }
}
