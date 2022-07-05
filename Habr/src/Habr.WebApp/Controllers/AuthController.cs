using Habr.BusinessLogic.Services.UserServices.Interfaces;
using Habr.Common.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Habr.WebApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/auth")]
    public class AuthController : UserIdController
    {
        private readonly IUserLoginService _userService;

        public AuthController(IUserLoginService service)
        {
            _userService = service;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> RegistrationAsync([FromBody] RegistrationUserDto registryUser,
                                                          CancellationToken cancellationToken = default)
        {
            var user = await _userService.RegistrationAsync(registryUser, cancellationToken);
            return CreatedAtAction(nameof(SignInAsync), user);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<UserWithTokenDto> SignInAsync([FromBody] LoginUserDto loginUser,
                                                        CancellationToken cancellationToken = default) =>
            await _userService.SignInAsync(loginUser.Email, loginUser.Password, cancellationToken);


        [HttpGet]
        [Route("name")]
        public async Task<String> GetCurrentUserNameAsync(CancellationToken cancellationToken = default) =>
            await _userService.GetUserNameAsync(UserID, cancellationToken);
    }
}
