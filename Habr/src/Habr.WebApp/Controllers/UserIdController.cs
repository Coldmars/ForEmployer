using Habr.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Habr.WebApp.Controllers
{
    public class UserIdController : ControllerBase
    {
        protected int UserID => GetUserId();

        private int GetUserId()
        {
            var claim = HttpContext.User.FindFirst("UserId");

            if (claim == null)
                throw new UnauthorizedException();

            return int.Parse(claim.Value);
        }
    }
}
