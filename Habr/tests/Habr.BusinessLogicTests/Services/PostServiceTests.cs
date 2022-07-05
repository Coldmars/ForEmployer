using AutoMapper;
using Habr.BusinessLogic.Services.PostServices;
using Habr.BusinessLogicTests.Helpers;
using Habr.Common.DTOs.Posts;
using Habr.Common.Exceptions;
using Habr.Common.Helpers.AutoMapper;
using Habr.Common.Resourses;
using Habr.DataAccess.Data;
using Habr.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Habr.BusinessLogicTests.Services
{
    public class PostServiceTests
    {
        private readonly DbContextOptions<DataContext> _dbContextOptions;
        private readonly ILogger<PostService> _logger;
        private readonly IMapper _mapper;
        private readonly TestEntitiesCreator _entitiesCreator;
        private const int TitleMaxLength = 200;
        private const int TextMaxLength = 2000;

        public PostServiceTests()
        {
            _logger = new Mock<ILogger<PostService>>().Object;
            _mapper = GetPostMapper();
            _dbContextOptions = ConfigureInMemoryDatabase();
            _entitiesCreator = new TestEntitiesCreator();
        }

        private CancellationToken CancellationToken = default;

        #region CreationPost
        [Fact]
        public async Task CreationPost_ValidPost_Added()
        {
            // Arrange
            var context = new DataContext(_dbContextOptions);
            var user = await AddTestUserToContextAsync(context);
            var draftpostDto = _entitiesCreator.GetDraftPostDto(user.Id);
            var postService = new PostService(context, _logger, _mapper);

            // Act
            var expectedPostDto = await postService.PublishPostAsync(draftpostDto, CancellationToken);
            var actualPostsCollection = _mapper.Map<IEnumerable<PostDto>>(context.Posts);
            await context.DisposeAsync();

            // Assert
            Assert.Contains(expectedPostDto, actualPostsCollection);
        }

        [Fact]
        public async Task CreationPost_TitleIsNull_ThrowValidationException()
        {
            // Arrange
            var context = new DataContext(_dbContextOptions);
            var user = await AddTestUserToContextAsync(context);
            var draftpostDto = _entitiesCreator.GetDraftPostWithoutTitle(user.Id);
            var postService = new PostService(context, _logger, _mapper);

            // Act
            var ex = await Assert.ThrowsAsync<ValidationException>(
                                    async () =>
                                        await postService.PublishPostAsync(draftpostDto, CancellationToken));
            var expectedExMessage = String.Format(ExceptionMessagesResourse.InvalidTitleLength, nameof(Post), TitleMaxLength);
            await context.DisposeAsync();

            // Assert
            Assert.Contains(expectedExMessage, ex.Message);
        }

        [Fact]
        public async Task CreationPost_TitleIsOverMax_ThrowValidationException()
        {
            // Arrange
            var context = new DataContext(_dbContextOptions);
            var user = await AddTestUserToContextAsync(context);
            var draftpostDto = _entitiesCreator.GetDraftPostWhereTitleIsOverMax(user.Id, TitleMaxLength);
            var postService = new PostService(context, _logger, _mapper);
            
            // Act
            var ex = await Assert.ThrowsAsync<ValidationException>(
                                    async () =>
                                        await postService.PublishPostAsync(draftpostDto, CancellationToken));
            var expectedExMessage = String.Format(ExceptionMessagesResourse.InvalidTitleLength, nameof(Post), TitleMaxLength);
            await context.DisposeAsync();

            // Assert
            Assert.Contains(expectedExMessage, ex.Message);
        }

        [Fact]
        public async Task CreationPost_TextIsNull_ThrowValidationException()
        {
            // Arrange
            var context = new DataContext(_dbContextOptions);
            var user = await AddTestUserToContextAsync(context);
            var draftpostDto = _entitiesCreator.GetDraftPostWithoutText(user.Id);
            var postService = new PostService(context, _logger, _mapper);
            
            // Act
            var ex = await Assert.ThrowsAsync<ValidationException>(
                                    async () =>
                                        await postService.PublishPostAsync(draftpostDto, CancellationToken));
            var expectedExMessage = String.Format(ExceptionMessagesResourse.InvalidFieldLength, nameof(Post), TextMaxLength);
            await context.DisposeAsync();

            // Assert
            Assert.Contains(expectedExMessage, ex.Message);
        }

        [Fact]
        public async Task CreationPost_TextIsOverMax_ThrowValidationException()
        {
            // Arrange
            var context = new DataContext(_dbContextOptions);
            var user = await AddTestUserToContextAsync(context);
            var draftpostDto = _entitiesCreator.GetDraftPostWhereTextIsOverMax(user.Id, TextMaxLength);
            var postService = new PostService(context, _logger, _mapper);
            
            // Act
            var ex = await Assert.ThrowsAsync<ValidationException>(
                                    async () =>
                                        await postService.PublishPostAsync(draftpostDto, CancellationToken));
            var expectedExMessage = String.Format(ExceptionMessagesResourse.InvalidFieldLength, nameof(Post), TextMaxLength);
            await context.DisposeAsync();

            // Assert
            Assert.Contains(expectedExMessage, ex.Message);
        }
        #endregion

        #region DeletionPost
        [Fact]
        public async void RemovePost_AddedPost_Deleted()
        {
            // Arrange
            var context = new DataContext(_dbContextOptions);
            var user = await AddTestUserToContextAsync(context);
            var post = await AddTestPostToContext(context, user.Id);
            var postService = new PostService(context, _logger, _mapper);

            // Act
            await postService.RemovePostAsync(post.Id, user.Id, CancellationToken);
            var expectedFalseResult = await context.Posts.AnyAsync(p => p.Id == post.Id);
            await context.DisposeAsync();

            // Assert
            Assert.False(expectedFalseResult);
        }

        [Fact]
        public async void RemovePost_NotExistPost_ThrowNotFoundException()
        {
            // Arrange
            var context = new DataContext(_dbContextOptions);
            var user = await AddTestUserToContextAsync(context);
            var post = await AddTestPostToContext(context, user.Id);
            var postService = new PostService(context, _logger, _mapper);
            var invalidPostId = _entitiesCreator.GetInvalidPostId(context);
            
            // Act
            var ex = await Assert.ThrowsAsync<NotFoundException>(
                                    async () =>
                                        await postService.RemovePostAsync(invalidPostId, user.Id, CancellationToken));
            var expectedExMessage = String.Format(ExceptionMessagesResourse.NotFound, nameof(Post));
            await context.DisposeAsync();

            // Assert
            Assert.Contains(expectedExMessage, ex.Message);
        }

        [Fact]
        public async void RemovePost_InvalidUserOfPost_ThrowUnauthorizedException()
        {
            // Arrange
            var context = new DataContext(_dbContextOptions);
            var user = await AddTestUserToContextAsync(context);
            var post = await AddTestPostToContext(context, user.Id);
            var postService = new PostService(context, _logger, _mapper);
            var invalidUserId = user.Id + 1;

            // Act
            var ex = await Assert.ThrowsAsync<NotFoundException>(
                                    async () =>
                                        await postService.RemovePostAsync(post.Id, invalidUserId, CancellationToken));
            var expectedExMessage = String.Format(ExceptionMessagesResourse.NotFound, nameof(Post));
            await context.DisposeAsync();

            // Assert
            Assert.Contains(expectedExMessage, ex.Message);
        }
        #endregion

        private DbContextOptions<DataContext> ConfigureInMemoryDatabase()
        {
            var dbName = $"db_{DateTime.Now}";

            return new DbContextOptionsBuilder<DataContext>()
                            .UseInMemoryDatabase<DataContext>(dbName)
                            //.ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning)
                            .Options;
        }

        private IMapper GetPostMapper()
        {
            if (_mapper is not null)
                return _mapper;

            var mappingConfig = new MapperConfiguration(
                mc =>
                {
                    mc.AddProfile(new PostProfile());
                });

            return mappingConfig.CreateMapper();
        }

        private async Task<User> AddTestUserToContextAsync(DataContext context)
        {
            var user = _entitiesCreator.GetTestUser();

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return user;
        }

        private async Task<Post> AddTestPostToContext(DataContext context, int authorId)
        {
            var draftpostDto = _entitiesCreator.GetDraftPostDto(authorId);
            var post = _mapper.Map<Post>(draftpostDto);

            await context.Posts.AddAsync(post);
            await context.SaveChangesAsync();

            return post;
        }
    }
}
