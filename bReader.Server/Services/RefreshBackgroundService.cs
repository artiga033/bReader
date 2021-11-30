using bReader.Shared.Services;

namespace bReader.Server.Services
{
    public class RefreshBackgroundService : BackgroundService
    {
        private readonly ISettingService _settingService;
        private readonly IFeedService _feedService;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int period;
            bool parseSuccess;
            do
            {
                parseSuccess = int.TryParse((await _settingService.GetAllSettingsAsync())["SourceUpdatePeriod"], out period);
                await _feedService.RefreshAllFeedsAsync(stoppingToken);
                await Task.Delay(TimeSpan.FromMinutes(period), stoppingToken);
            } while (parseSuccess && !stoppingToken.IsCancellationRequested);
        }
        public RefreshBackgroundService(ISettingService settingService, AutoMapper.IMapper mapper, Microsoft.EntityFrameworkCore.IDbContextFactory<Data.FeedDbContext> factory)
        {
            this._settingService = settingService;
            this._feedService = new FeedDataService(mapper, factory, _settingService);
        }
    }
}
