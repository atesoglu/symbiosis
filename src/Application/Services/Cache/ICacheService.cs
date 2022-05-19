namespace Application.Services.Cache
{
    public interface ICacheService
    {
        Task<string> GetAsync(string key, CancellationToken cancellationToken);
        Task SaveAsync(string key, string value, TimeSpan timeToLive, CancellationToken cancellationToken);
        Task RemoveAsync(string key, CancellationToken cancellationToken);
    }
}