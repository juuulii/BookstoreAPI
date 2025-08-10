using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _context; //el contexto en realidad se hacen en las Interfaces?
        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }
        public User? Get(string name)
        {
            return _context.Users.FirstOrDefault(u => u.Name == name);
        }

        public List<User> Get()
        {
            return _context.Users.ToList();
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
        }

        public User GetById(int id)
        {
            return _context.Users.First(u => u.Id == id);
        }

        public int AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user.Id;

        }
    }
}

