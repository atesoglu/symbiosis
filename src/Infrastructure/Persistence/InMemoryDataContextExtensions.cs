using System.Diagnostics;
using Application.Persistence;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence
{
    /// <summary>
    /// In-memory database extensions.
    /// </summary>
    public static class InMemoryDataContextExtensions
    {
        /// <summary>
        /// Seeds the in-memory database with required entities.
        /// </summary>
        /// <param name="dbContext">IDataContext instance to be seeded.</param>
        /// <param name="logger">Logger implementation.</param>
        public static void SeedData(this IDataContext dbContext, ILogger logger)
        {
            logger.LogInformation("Seeding in-memory database context");

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            for (var i = 0; i < 1000; i++)
            {
                dbContext.Users.Add(new UserModel {Email = $"{Guid.NewGuid():N}@a.com"});
            }

            dbContext.SaveChangesAsync(CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult();

            stopWatch.Stop();

            var count = dbContext.Users.Count();
            var elapsed = stopWatch.ElapsedMilliseconds / 1000m;
            logger.LogInformation("There are total {count} locations persisted in database. Time elapsed: {elapsed} seconds.", count, elapsed);
        }
    }
}