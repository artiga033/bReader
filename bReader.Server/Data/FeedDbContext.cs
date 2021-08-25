using bReader.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bReader.Server.Data
{
    public class FeedDbContext : DbContext
    {
        public FeedDbContext(DbContextOptions<FeedDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Feed>().HasIndex(x => x.SubscribeLink).IsUnique();
            modelBuilder.Entity<FeedItem>().HasOne(X => X.SourceFeed).WithMany(x => x.Items).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<FeedGroup>().HasData(new FeedGroup() { Id = -1, Name = "默认" });
            modelBuilder.Entity<AppSetting>().HasIndex(x => x.Key).IsUnique();
            // https://blog.dangl.me/archive/handling-datetimeoffset-in-sqlite-with-entity-framework-core/
            // SQLite does not have proper support for DateTimeOffset via Entity Framework Core, see the limitations
            // here: https://docs.microsoft.com/en-us/ef/core/providers/sqlite/limitations#query-limitations
            // To work around this, when the Sqlite database provider is used, all model properties of type DateTimeOffset
            // use the DateTimeOffsetToBinaryConverter
            // Based on: https://github.com/aspnet/EntityFrameworkCore/issues/10784#issuecomment-415769754
            // This only supports millisecond precision, but should be sufficient for most use cases.
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(DateTimeOffset)
                                                                            || p.PropertyType == typeof(DateTimeOffset?));
                foreach (var property in properties)
                {
                    modelBuilder
                        .Entity(entityType.Name)
                        .Property(property.Name)
                        .HasConversion(new DateTimeOffsetToBinaryConverter());
                }
            }
        }
        public DbSet<Feed> Feeds { get; set; }
        public DbSet<FeedItem> FeedItems { get; set; }
        public DbSet<FeedGroup> Groups { get; set; }
        public DbSet<AppSetting> Settings { get; set; }
    }
}
