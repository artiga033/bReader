namespace bReader.Shared.Services
{
    public interface ISettingService
    {
        public Task<Dictionary<string, string>> GetAllSettingsAsync();
        public Task<string> GetSettingAsync(string key);
        public Task<bool> SaveSettingsAsync(Dictionary<string, string> settings);
    }
}
