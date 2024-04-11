using EloboostCommerce.Models.Classes;
using EloboostCommerce.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EloboostCommerce.Services
{
    public interface IUserService
    {
        User GetById(int userId);
    }
    public class UserService:IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User GetById(int userId)
        {
            return _userRepository.GetById(userId);
        }
    }
}
