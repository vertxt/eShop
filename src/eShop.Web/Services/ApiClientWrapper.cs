using System.Net;
using System.Text.Json;
using eShop.Web.Exceptions;
using eShop.Web.Extensions;
using eShop.Web.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Web.Services
{
    public class ApiClientWrapper : IApiClientWrapper
    {
        private readonly ILogger<ApiClientWrapper> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiClientWrapper(ILogger<ApiClientWrapper> logger,
                               IHttpContextAccessor httpContextAccessor,
                               IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<T> GetAsync<T>(string endpoint, bool requiresAuth = false) where T : class, new()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("API");

                if (requiresAuth)
                {
                    client = await client.AddTokenAsync(_httpContextAccessor);
                }

                var response = await client.GetAsync(endpoint);
                await EnsureSuccessStatusCodeAsync(response);

                var content = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<T>(content, _jsonOptions) ?? new T();
            }
            catch (ApiException)
            {
                throw; // Re-throw ApiExceptions as they're already handled
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during GET request to {Endpoint}", endpoint);
                throw new ApiException("An error occurred while processing your request", ex);
            }
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data, bool requiresAuth = true)
            where TResponse : class, new()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("API");

                if (requiresAuth)
                {
                    client = await client.AddTokenAsync(_httpContextAccessor);
                }

                var response = await client.PostAsJsonAsync(endpoint, data);
                await EnsureSuccessStatusCodeAsync(response);

                var content = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<TResponse>(content, _jsonOptions) ?? new TResponse();
            }
            catch (ApiException)
            {
                throw; // Re-throw ApiExceptions as they're already handled
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during POST request to {Endpoint}", endpoint);
                throw new ApiException("An error occurred while processing your request", ex);
            }
        }


        public async Task<TResponse> PutAsync<TRequest, TResponse>(string endpoint, TRequest data, bool requiresAuth = true)
            where TResponse : class, new()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("API");

                if (requiresAuth)
                {
                    client = await client.AddTokenAsync(_httpContextAccessor);
                }

                var response = await client.PutAsJsonAsync(endpoint, data);
                await EnsureSuccessStatusCodeAsync(response);

                var content = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<TResponse>(content, _jsonOptions) ?? new TResponse();
            }
            catch (ApiException)
            {
                throw; // Re-throw ApiExceptions as they're already handled
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during PUT request to {Endpoint}", endpoint);
                throw new ApiException("An error occurred while processing your request", ex);
            }
        }

        public async Task<T> DeleteAsync<T>(string endpoint, bool requiresAuth = false) where T : class, new()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("API");

                if (requiresAuth)
                {
                    client = await client.AddTokenAsync(_httpContextAccessor);
                }

                var response = await client.DeleteAsync(endpoint);
                await EnsureSuccessStatusCodeAsync(response);

                var content = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<T>(content, _jsonOptions) ?? new T();
            }
            catch (ApiException)
            {
                throw; // Re-throw ApiExceptions as they're already handled
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during DELETE request to {Endpoint}", endpoint);
                throw new ApiException("An error occurred while processing your request", ex);
            }
        }

        private async Task EnsureSuccessStatusCodeAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            var statusCode = response.StatusCode;
            string errorMessage;

            try
            {
                // Try to parse ProblemDetails from the API
                var content = await response.Content.ReadAsStringAsync();
                var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(content, _jsonOptions);
                errorMessage = problemDetails?.Detail ?? problemDetails?.Title ?? "Unknown error occurred";
            }
            catch
            {
                errorMessage = "Error communicating with the service";
            }

            throw statusCode switch
            {
                HttpStatusCode.NotFound => new NotFoundException(errorMessage),
                HttpStatusCode.Unauthorized => new UnauthorizedException(errorMessage),
                HttpStatusCode.Forbidden => new ForbiddenException(errorMessage),
                HttpStatusCode.BadRequest => new BadRequestException(errorMessage),
                _ => new ApiException($"API Error: {errorMessage}", statusCode: (int)statusCode)
            };
        }
    }
}