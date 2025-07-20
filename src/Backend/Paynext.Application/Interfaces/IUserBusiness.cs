using Paynext.Application.Dtos.Entities.User;
using Paynext.Application.Interfaces._bases;
using Paynext.Domain.Entities;
using SharedKernel;

namespace Paynext.Application.Interfaces
{
    public interface IUserBusiness : IBusinessBase<User, CreateUserDto, UpdateUserDto, UserDto>
    {
        Task<Response<User>> Get(string email);
        Task<Response<bool>> ChangePasswordAsync(ChangePasswordDto changePasswordDto);
        Task<Response<IEnumerable<UserDto>>> GetAll();
        Task<Response<IEnumerable<UserDto>>> GetAllByRole(string role);

    }
}
