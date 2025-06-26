using ForumService.Application.DTO;
using ForumService.Core.Entities;
using ForumService.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumService.Application.Services
{
    public class ForumCommentService : IForumCommentService
    {
        private readonly IForumCommentRepository _repo;
        private readonly IUserInfoClient _userInfoClient;
        public ForumCommentService(IForumCommentRepository repo, IUserInfoClient userInfoClient)
        {
            _repo = repo;
            _userInfoClient = userInfoClient;
        }

        public async Task<IEnumerable<ForumCommentDto>> GetByForumIdAsync(int forumId)
        {
            var comments = await _repo.GetByForumIdAsync(forumId);
            return comments.Select(c => new ForumCommentDto
            {
                Id = c.Id,
                ForumId = c.ForumId,
                AuthorId = c.AuthorId,
                Content = c.Content,
                CreatedAt = c.CreatedAt
            });
        }

        public async Task<ForumCommentDto?> GetByIdAsync(int id)
        {
            var c = await _repo.GetByIdAsync(id);
            if (c == null) return null;
            return new ForumCommentDto
            {
                Id = c.Id,
                ForumId = c.ForumId,
                AuthorId = c.AuthorId,
                Content = c.Content,
                CreatedAt = c.CreatedAt
            };
        }

        public async Task<ForumCommentDto> CreateAsync(CreateForumCommentRequest request)
        {
            var comment = new ForumComment
            {
                ForumId = request.ForumId,
                AuthorId = request.AuthorId,
                Content = request.Content,
                CreatedAt = System.DateTime.UtcNow
            };
            await _repo.AddAsync(comment);
            return new ForumCommentDto
            {
                Id = comment.Id,
                ForumId = comment.ForumId,
                AuthorId = comment.AuthorId,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var comment = await _repo.GetByIdAsync(id);
            if (comment == null) return false;
            await _repo.DeleteAsync(id);
            return true;
        }

        public async Task<IEnumerable<ForumCommentWithAuthorDto>> GetWithAuthorsByForumIdAsync(int forumId)
        {
            var comments = await _repo.GetByForumIdAsync(forumId);
            var commentsList = comments.ToList();
            var userIds = commentsList
                .Where(c => !string.IsNullOrEmpty(c.AuthorId))
                .Select(c => c.AuthorId)
                .Distinct()
                .ToList();
            var users = await _userInfoClient.GetUsersBatchAsync(userIds);
            var usersDict = users.ToDictionary(u => u.Id, u => u);
            var result = new List<ForumCommentWithAuthorDto>();
            foreach (var c in commentsList)
            {
                var authorKey = Math.Abs((c.AuthorId ?? "").GetHashCode());
                usersDict.TryGetValue(authorKey, out var author);
                result.Add(new ForumCommentWithAuthorDto
                {
                    Id = c.Id,
                    ForumId = c.ForumId,
                    Author = author ?? new UserShortDto { Id = 0, Nickname = "Неизвестно" },
                    Content = c.Content,
                    CreatedAt = c.CreatedAt
                });
            }
            return result;
        }
    }
} 