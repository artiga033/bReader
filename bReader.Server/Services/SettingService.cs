using bReader.Server.Data;
using bReader.Shared.Models;
using bReader.Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace bReader.Server.Services
{
    public class SettingService : ISettingService
    {
        private readonly IDbContextFactory<FeedDbContext> _factory;
        /// <summary>
        /// For each instance, here we cache a dictionary so as to reduce db query. Everytime <see cref="SaveSettingsAsync(Dictionary{string, string})"/>
        /// was called, which means the db data has got updated, the cache would be updated too.
        /// </summary>
        private Dictionary<string, string> cached;
        public async Task<Dictionary<string, string>> GetAllSettingsAsync()
        {
            if (cached != null)
                return cached;
            using var context = _factory.CreateDbContext();
            var sets = await context.Settings.AsNoTracking().ToDictionaryAsync(x => x.Key, x => x.Value);
            this.cached = sets;
            return sets;
        }
        public async Task<string> GetSettingAsync(string key) => (await this.GetAllSettingsAsync())[key];

        public async Task<bool> SaveSettingsAsync(Dictionary<string, string> settings)
        {
            using var context = _factory.CreateDbContext();
            //use a lookuptable exchanging space with time
            var lookup = await context.Settings.AsNoTracking().ToDictionaryAsync(x => x.Key, x => x.Id);
            var sets = settings.Select(x => new AppSetting() { Id = lookup.ContainsKey(x.Key) ? lookup[x.Key] : 0, Key = x.Key, Value = x.Value });
            context.UpdateRange(sets);
            this.cached = settings;// updated cached data
            return await context.SaveChangesAsync() > 0;
        }
        public SettingService(IDbContextFactory<FeedDbContext> factory)
        {
            this._factory = factory;
        }
    }
}
