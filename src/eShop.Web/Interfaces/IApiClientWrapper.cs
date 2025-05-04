namespace eShop.Web.Interfaces
{
    public interface IApiClientWrapper
    {
        Task<T> GetAsync<T>(string endpoint, bool requiresAuth = false) where T : class, new();
        Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data, bool requiresAuth = true) where TResponse : class, new();
        Task<TResponse> PutAsync<TRequest, TResponse>(string endpoint, TRequest data, bool requiresAuth = true) where TResponse : class, new();
        Task<T> DeleteAsync<T>(string endpoint, bool requiresAuth = true) where T : class, new();
    }
}