using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ForumService.Application.DTO;
using ForumService.Application.Services;
using System.Collections.Generic;
using System.Linq;

namespace ForumService.Infrastructure
{
    public class UserInfoClient : IUserInfoClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://localhost:60197/api/users";
        public UserInfoClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<UserShortDto?> GetUserShortAsync(string userId)
        {
            try
            {
                var user = await _httpClient.GetFromJsonAsync<UserInfoDto>($"{_baseUrl}/{userId}");
                if (user == null) return null;
                
                var numericId = Math.Abs(user.Id.GetHashCode());
                
                return new UserShortDto
                {
                    Id = numericId,
                    Nickname = user.Nickname
                };
            }
            catch
            {
                return null;
            }
        }
        public async Task<IEnumerable<UserShortDto>> GetUsersBatchAsync(IEnumerable<string> userIds)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/batch", userIds);
                var users = await response.Content.ReadFromJsonAsync<IEnumerable<UserInfoDto>>();
                return users?.Select(u => new UserShortDto
                {
                    Id = Math.Abs(u.Id.GetHashCode()),
                    Nickname = u.Nickname
                }) ?? Enumerable.Empty<UserShortDto>();
            }
            catch
            {
                return Enumerable.Empty<UserShortDto>();
            }
        }
    }
} 