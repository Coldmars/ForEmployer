using Habr.Common.DTOs.User;

namespace Habr.BusinessLogic.Services.UserServices.Interfaces
{
    public interface IUserLoginService
    {
        Task<UserDto> RegistrationAsync(RegistrationUserDto userDto,
                               CancellationToken cancellationToken);

        Task<UserWithTokenDto> SignInAsync(string email,
                                           string password,
                                           CancellationToken cancellationToken);

        Task<String> GetUserNameAsync(int userId,
                                 CancellationToken cancellationToken);
    }
}
