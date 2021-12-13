using Application.Persistence;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    /// <summary>
    /// In-memory implementation of IDataContext which extends DbContext
    /// </summary>
    public class InMemoryDataContext : DbContext, IDataContext
    {
        public DbSet<UserModel> Users { get; set; }

        public InMemoryDataContext(DbContextOptions<InMemoryDataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder
                .Entity<UserModel>(entity => entity.HasKey(t => t.UserId))
                ;
        }
    }
}