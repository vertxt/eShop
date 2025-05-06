using eShop.Business.Interfaces;
using eShop.Data.Entities.UserAggregate;
using eShop.Shared.DTOs.Users;
using Microsoft.AspNetCore.Identity;

namespace eShop.Business.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = _userManager.Users.ToList();
            var result = new List<UserDto>(users.Count);

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                result.Add(new UserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = roles,
                    JoinedDate = user.JoinedDate.ToString("dd/MM/yyyy"),
                });
            }

            return result;
        }
    }
}