using Habr.BusinessLogic.Services.PostServices.Interfaces;
using Habr.Common.DTOs.Posts;
using Habr.DataAccess.Data;
using Habr.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Habr.Common.Exceptions;
using AutoMapper;
using Habr.BusinessLogic.Extensions;
using Habr.Common.Resourses;
using Habr.BusinessLogic.Validation;

namespace Habr.BusinessLogic.Services.PostServices
{
    public class DraftService : IDraftService
    {
        private readonly DataContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public DraftService(
            DataContext context, 
            ILogger<DraftService> logger, 
            IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DraftDto>> GetDraftsByUserIdAsync(int userId,
                                                                        CancellationToken cancellationToken)
        {
            return await _context
                .Drafts
                    .GetAllOrderedDraftsByUserId(userId)
                    .ToListAsync(cancellationToken);
        }
            
        public async Task<DraftDto> AddDraftAsync(DraftPostDto postDto,
                                                  CancellationToken cancellationToken)
        {
            LengthValidator.PostValidate(postDto.Title, postDto.Text);

            var draft = _mapper.Map<Draft>(postDto);
            draft.Created = DateTime.Now;

            await AddDraftToContextAsync(draft, cancellationToken);

            return _mapper.Map<DraftDto>(draft);
        }

        public async Task<DraftDto> MovePostToDraftAsync(int postId,
                                                         int userId,
                                                         CancellationToken cancellationToken)
        {
            var movedPost = await _context
                .Posts
                    .GetPostWithComments(postId)
                    .SingleOrDefaultAsync();
                    
            GuardAgaintInvalidPost(movedPost, userId);
            GuardAgainstExistanceCommentsException(movedPost);

            var draft = _mapper.Map<Draft>(movedPost);

            await MovePostToDraftInContextAsync(movedPost, draft, cancellationToken);

            return _mapper.Map<DraftDto>(draft);
        }
        
        public async Task<PostDto> PublishDraftAsync(int draftId,
                                            CancellationToken cancellationToken)
        {
            var draft = await _context
                .Drafts
                    .FindAsync(draftId);

            GuardAgainstDraftNotFoundException(draft);

            var post = _mapper.Map<Post>(draft);

            LengthValidator.PostValidate(post.Title, post.Text);

            await MoveDraftToPostInContextAsync(draft, post, cancellationToken);

            return _mapper.Map<PostDto>(post);
        }

        public async Task<DraftDto> EditDraftAsync(ChangingDraftDto edited,
                                                   int draftId,
                                                   int userId,
                                                   CancellationToken cancellationToken)
        {
            var draft = await _context
                .Drafts
                    .FindAsync(draftId);

            GuardAgainstInvalidDraft(draft, userId);

            draft = ChangeDraft(draft, edited);

            await UpdateDraftContextAsync(draft, cancellationToken);

            return _mapper.Map<DraftDto>(draft);
        }

        public async Task RemoveDraftAsync(int draftId,
                                           int userId,
                                           CancellationToken cancellationToken)
        {
            var draft = await _context
                .Drafts
                    .FindAsync(draftId);

            GuardAgainstInvalidDraft(draft, userId);

            await RemoveDraftFromContextAsync(draft, cancellationToken);
        }

        private Draft ChangeDraft(Draft destDraft, ChangingDraftDto srcDraft)
        {
            destDraft.Title = srcDraft.Title;
            destDraft.Text = srcDraft.Text;
            destDraft.LastUpdated = DateTime.Now;

            return destDraft;
        }

        private void GuardAgainstInvalidDraft(Draft draft, int userId)
        {
            GuardAgainstDraftNotFoundException(draft);
            GuardAgainstInvalidDraftsUserException(draft.UserId, userId);
        }

        private void GuardAgaintInvalidPost(Post post, int userId)
        {
            GuardAgainstPostNotFoundException(post);
            GuardAgainstInvalidDraftsUserException(post.UserId, userId);
        }

        private void GuardAgainstDraftNotFoundException(Draft draft)
        {
            if (draft is null)
                throw new NotFoundException(String.Format(ExceptionMessagesResourse.NotFound, nameof(Draft)));
        }

        private void GuardAgainstPostNotFoundException(Post post)
        {
            if (post is null)
                throw new NotFoundException(String.Format(ExceptionMessagesResourse.NotFound, nameof(Post)));
        }

        private void GuardAgainstInvalidDraftsUserException(int? userIdInPost,
                                                            int userId)
        {
            if (userIdInPost != userId)
                throw new ForbiddenException(ExceptionMessagesResourse.InvalidDraftsUser);
        }

        private void GuardAgainstExistanceCommentsException(Post post)
        {
            if (post.Comments.Count > 0)
                throw new BusinessLogicException(ExceptionMessagesResourse.PostHasComments);
        }

        private async Task AddDraftToContextAsync(Draft draft,
                                                  CancellationToken cancellationToken)
        {
            await _context.Drafts.AddAsync(draft);
            await _context.SaveAsync(cancellationToken);
        }

        private async Task MovePostToDraftInContextAsync(Post post,
                                                         Draft draft,
                                                         CancellationToken cancellationToken)
        {
            await _context.BeginTransactionAsync();
            _context.Posts.Remove(post);
            await _context.Drafts.AddAsync(draft);
            await _context.CommitAsync(cancellationToken);
        }

        private async Task MoveDraftToPostInContextAsync(Draft draft,
                                                         Post post,
                                                         CancellationToken cancellationToken)
        {
            await _context.BeginTransactionAsync();
            _context.Drafts.Remove(draft);
            await _context.Posts.AddAsync(post);
            await _context.CommitAsync(cancellationToken);
        }

        private async Task UpdateDraftContextAsync(Draft draft,
                                                   CancellationToken cancellationToken)
        {
            _context.Drafts.Update(draft);
            await _context.SaveAsync(cancellationToken);
        }

        private async Task RemoveDraftFromContextAsync(Draft draft,
                                                       CancellationToken cancellationToken)
        {
            _context.Drafts.Remove(draft);
            await _context.SaveAsync(cancellationToken);
        }
    }
}
