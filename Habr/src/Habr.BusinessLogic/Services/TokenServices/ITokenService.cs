using Habr.DataAccess.Entities;

namespace Habr.BusinessLogic.Services.TokenServices
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
