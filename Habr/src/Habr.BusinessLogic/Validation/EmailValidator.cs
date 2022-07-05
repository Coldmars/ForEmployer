using Habr.Common.Exceptions;
using Habr.Common.Resourses;

namespace Habr.BusinessLogic.Validation
{
    public static class EmailValidator
    {
        public static void Validate(string email)
        {
            try
            {
                System.Net.Mail.MailAddress m = new(email);
            }
            catch (FormatException)
            {
                throw new ValidationException(ExceptionMessagesResourse.InvalidEmail);
            }
        }
    }
}
