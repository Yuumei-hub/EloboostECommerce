using EloboostCommerce.Models.Classes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EloboostCommerce.Repositories
{
    public interface IUserRepository
    {
        User GetById(int userId);
    }
    public class UserRepository : IUserRepository
    {
        private readonly Context _context;
        public UserRepository(Context context)
        {
            _context = context;
        }
        User IUserRepository.GetById(int userId)
        {
            return _context.Users.FirstOrDefault(u=>u.UserId==userId);
        }
    }
}
