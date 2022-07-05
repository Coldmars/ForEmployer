using AutoMapper;
using Habr.BusinessLogic.Extensions;
using Habr.BusinessLogic.Services.TokenServices;
using Habr.BusinessLogic.Services.UserServices.Interfaces;
using Habr.BusinessLogic.Validation;
using Habr.Common.DTOs.User;
using Habr.Common.Exceptions;
using Habr.Common.Resourses;
using Habr.DataAccess.Data;
using Habr.DataAccess.Entities;
using Habr.Security.Helpers.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Habr.BusinessLogic.Services.UserServices
{
    public class UserLoginService : IUserLoginService
    {
        private readonly DataContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;

        public UserLoginService(
            DataContext context, 
            ILogger<UserLoginService> logger, 
            IMapper mapper,
            ITokenService tokenService,
            IPasswordHasher passwordHasher)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserDto> RegistrationAsync(RegistrationUserDto registryUserDto,
                                                     CancellationToken cancellationToken)
        {
            GuardAgainstIncorrectOrTakenEmail(registryUserDto.Email);

            var user = _mapper.Map<User>(registryUserDto);
            user.PasswordHash = _passwordHasher.GetHashString(registryUserDto.Password);
            
            await AddUserToContextAsync(user, cancellationToken);
            _logger.LogInformation(String.Format(LogMessagesResourse.Registration, user.Id));

            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }

        public async Task<UserWithTokenDto> SignInAsync(string email,
                                                        string password,
                                                        CancellationToken cancellationToken)
        {
            var user = await _context
                .Users
                .GetUserByEmail(email)
                .SingleOrDefaultAsync();

            GuardAgainstCredentialsNotFound(user, password);

            _logger.LogInformation(String.Format(LogMessagesResourse.Registration, user.Id));

            var userWithToken = _mapper.Map<UserWithTokenDto>(user);
            userWithToken.Token = _tokenService.CreateToken(user);

            return userWithToken;
        }

        public async Task<String> GetUserNameAsync(int userId, 
                                              CancellationToken cancellationToken)
        {
            var userName = await _context
                .Users
                    .GetUserNameById(userId)
                    .SingleOrDefaultAsync();

            if (userName is null)
                throw new NotFoundException(String.Format(ExceptionMessagesResourse.NotFound, nameof(User)));

            return userName;
        }

        private void GuardAgainstIncorrectOrTakenEmail(string email)
        {
            LengthValidator.EmailValidate(email);
            EmailValidator.Validate(email);
            GuardAgainstEmailTakenException(email);
        }

        private void GuardAgainstCredentialsNotFound(User user, string password)
        {
            if (user is null)
                throw new UnauthorizedException(ExceptionMessagesResourse.Unauthorized);

            var passwordHash = _passwordHasher.GetHashString(password);

            if (user.PasswordHash != passwordHash)
                throw new UnauthorizedException(ExceptionMessagesResourse.Unauthorized);
        }

        private void GuardAgainstEmailTakenException(string email)
        {
            var userWithThisEmail = _context.Users.SingleOrDefault(x => x.Email == email);

            if (userWithThisEmail is not null)
                throw new ValidationException(ExceptionMessagesResourse.EmailIsTaken);
        }

        private async Task AddUserToContextAsync(User user,
                                                 CancellationToken cancellationToken)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveAsync(cancellationToken);
        }
    }
}
