using Habr.Common.Exceptions;
using Habr.Common.Resourses;
using Habr.DataAccess.Entities;

namespace Habr.BusinessLogic.Validation
{
    public static class LengthValidator
    {
        private const int MaxPostTextLength = 2000;
        private const int MaxPostTitleLength = 200;
        private const int MaxCommentTextLength = 500;
        private const int MaxEmailLength = 200;

        public static void PostValidate(string title, string text)
        {
            TitleValidate(title, nameof(Post), MaxPostTitleLength);
            TextValidate(text, nameof(Post), MaxPostTextLength);
        }

        public static void DraftValidate(string title, string text)
        {
            TitleValidate(title, nameof(Draft), MaxPostTitleLength);
            TextValidate(text, nameof(Draft), MaxPostTextLength);
        }

        public static void EmailValidate(string email)
        {
            if (email.Length > MaxEmailLength)
                throw new ValidationException(String.Format(ExceptionMessagesResourse.InvalidFieldLength, nameof(email), MaxEmailLength));
        }

        public static void CommentValidate(string text)
        {
            TextValidate(text, nameof(Comment), MaxCommentTextLength);
        }

        private static void TitleValidate(string title, 
                                          string entity, 
                                          int maxLength)
        {
            if (title is null)
                throw new ValidationException(String.Format(ExceptionMessagesResourse.InvalidTitleLength, 
                                                            entity, 
                                                            maxLength));

            if (title.Length > MaxPostTitleLength)
                throw new ValidationException(String.Format(ExceptionMessagesResourse.InvalidTitleLength, 
                                                            entity, 
                                                            maxLength));
        }

        private static void TextValidate(string text, 
                                         string entity, 
                                         int maxLength)
        {
            if (text is null)
                throw new ValidationException(String.Format(ExceptionMessagesResourse.InvalidFieldLength, 
                                                            entity, 
                                                            maxLength));

            if (text.Length > MaxPostTextLength)
                throw new ValidationException(String.Format(ExceptionMessagesResourse.InvalidFieldLength, 
                                                            entity,     
                                                            maxLength));
        }
    }
}
