using AutoMapper;
using Habr.BusinessLogic.Extensions;
using Habr.BusinessLogic.Services.PostServices.Interfaces;
using Habr.BusinessLogic.Validation;
using Habr.Common.DTOs.Posts;
using Habr.Common.Exceptions;
using Habr.Common.Resourses;
using Habr.DataAccess.Data;
using Habr.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Habr.BusinessLogic.Services.PostServices
{
    public class PostService : IPostService
    {
        private readonly DataContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public PostService(
            DataContext context, 
            ILogger<PostService> logger, 
            IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PostDto> PublishPostAsync(DraftPostDto draftPostDto,
                                                    CancellationToken cancellationToken)
        {
            LengthValidator.PostValidate(draftPostDto.Title, draftPostDto.Text);

            var post = _mapper.Map<Post>(draftPostDto);
            post.Created = DateTime.Now;
            await AddPostToContextAsync(post, cancellationToken);

            _logger.LogInformation(String.Format(LogMessagesResourse.PublishPost, post.Id, post.UserId));

            var postDto = _mapper.Map<PostDto>(post);
            return postDto;
        }

        public async Task<IEnumerable<PublishedPostDto>> GetOrderedListOfPostsAsync(CancellationToken cancellationToken) 
        {
            return await _context
                .Posts
                    .GetAllOrderedPosts()
                    .ToListAsync(cancellationToken);
        }

        public async Task<PublishedPostDto> OpenSelectedPostAsync(int postId)
        {
            return await _context
                .Posts
                    .GetSelectedPost(postId)
                    .SingleOrDefaultAsync();
        }

        public async Task RemovePostAsync(int postId,
                                          int userId,
                                          CancellationToken cancellationToken)
        {
            var post = await _context
                .Posts
                    .SingleOrDefaultAsync(post => post.Id == postId && post.UserId == userId);

            GuardAgainstPostNotFoundException(post);

            await RemovePostFromContextAsync(post, cancellationToken);
        }

        private void GuardAgainstPostNotFoundException(Post post)
        {
            if (post is null)
                throw new NotFoundException(String.Format(ExceptionMessagesResourse.NotFound, nameof(Post)));
        }

        private async Task AddPostToContextAsync(Post post,
                                                 CancellationToken cancellationToken)
        {
            await _context.Posts.AddAsync(post);
            await _context.SaveAsync(cancellationToken);
        }

        private async Task RemovePostFromContextAsync(Post post,
                                                      CancellationToken cancellationToken)
        {
            _context.Posts.Remove(post);
            await _context.SaveAsync(cancellationToken);
        }
    }
}
