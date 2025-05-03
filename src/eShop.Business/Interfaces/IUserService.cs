using eShop.Shared.DTOs.Users;

namespace eShop.Business.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
    }
}