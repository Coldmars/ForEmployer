using AutoMapper;
using Habr.BusinessLogic.Services.CommentServices.Interfaces;
using Habr.Common.DTOs.Comments;
using Habr.Common.Exceptions;
using Habr.Common.Resourses;
using Habr.DataAccess.Data;
using Habr.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Habr.BusinessLogic.Services.CommentServices
{
    public class CommentService : ICommentService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CommentService(
             DataContext context,
             IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CommentDto> LeaveCommentAsync(AddCommentDto addCommentDto, 
                                                        CancellationToken cancellationToken)
        {
            GuardAgainstPostNotFoundExceptionAsync(addCommentDto.PostId);
            GuardAgainstCommentTextLengthException(addCommentDto.Text);

            var comment = _mapper.Map<Comment>(addCommentDto);
            comment.CreateDate = DateTime.Now;

            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync(cancellationToken);

            var commentDto = _mapper.Map<CommentDto>(comment);
            return commentDto;
        }

        public async Task<CommentDto> ReplyToCommentAsync(ReplyCommentDto replyCommentDto, 
                                                          CancellationToken cancellationToken)
        {
            GuardAgainstPostNotFoundExceptionAsync(replyCommentDto.PostId);
            GuardAgainstCommentTextLengthException(replyCommentDto.Text);
            _ = await FindCommentByIdAsync(replyCommentDto.ParentCommentId);

            var comment = _mapper.Map<Comment>(replyCommentDto);
            comment.CreateDate = DateTime.Now;

            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync(cancellationToken);

            var commentDto = _mapper.Map<CommentDto>(comment);
            return commentDto;
        }

        public async Task<CommentDto> GetCommentAsync(int commentId, 
                                                      CancellationToken cancellationToken)
        {
            var comment = await FindCommentByIdAsync(commentId);

            var commentDto = _mapper.Map<CommentDto>(comment);
            return commentDto;
        }

        public async Task DeleteCommentAsync(int commentId, 
                                             int userId, 
                                             CancellationToken cancellationToken)
        {
            var comment = await FindCommentByIdAsync(commentId);
            GuardAgainstInvalidAuthorException(comment.UserId, userId);
            GuardAgainstDeleteCommentWithChildExceptionAsync(comment.Id);

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync(cancellationToken);
        }  

        private async void GuardAgainstPostNotFoundExceptionAsync(int postId)
        {
            var post = await _context.Posts
                .SingleOrDefaultAsync(p => p.Id == postId);

            if (post is null)
                throw new NotFoundException(String.Format(ExceptionMessagesResourse.NotFound, nameof(Post)));
        }

        private void GuardAgainstCommentTextLengthException(string text)
        {
            const int MaxLength = 500;

            if (text is null)
                throw new ValidationException(String.Format(ExceptionMessagesResourse.InvalidFieldLength, nameof(Comment), MaxLength));

            if (text.Length > MaxLength)
                throw new ValidationException(String.Format(ExceptionMessagesResourse.InvalidFieldLength, nameof(Comment), MaxLength));
        }

        private async Task<Comment> FindCommentByIdAsync(int commentId)
        {
            var comment = await _context.Comments
                .SingleOrDefaultAsync(c => c.Id == commentId);

            if (comment is null)
                throw new NotFoundException(String.Format(ExceptionMessagesResourse.NotFound, nameof(Comment)));

            return comment;
        }

        private void GuardAgainstInvalidAuthorException(int commentsAuthorId, int userId)
        {
            if (commentsAuthorId != userId)
                throw new ForbiddenException(ExceptionMessagesResourse.InvalidCommentsUser);
        }

        private async void GuardAgainstDeleteCommentWithChildExceptionAsync(int commentId)
        {
            var child = await _context.Comments
                .FirstOrDefaultAsync(c => c.ParentCommentId == commentId);

            if (child is not null)
                throw new BusinessLogicException(ExceptionMessagesResourse.CommentHasChild);
        }
    }
}
