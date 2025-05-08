using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;

namespace eShop.Web.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpClient> AddTokenAsync(this HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor.HttpContext is not null)
            {
                var accessToken = await httpContextAccessor.HttpContext.GetTokenAsync("access_token");
                // Console.WriteLine("Access Token here: " + accessToken);
                // Console.WriteLine("Token is null or empty: " + string.IsNullOrEmpty(accessToken));
                if (!string.IsNullOrEmpty(accessToken))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                }
            }

            return client;
        }
    }
}