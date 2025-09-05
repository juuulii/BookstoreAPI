using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        User? Get(string name);
        List<User> Get();
        User GetById(int id);
        int AddUser(User user);
        void Update(User user);
        void Delete(int id); // 👈 agregado
    }
}
