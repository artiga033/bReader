using bReader.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bReader.Shared.Services
{
    public interface ISettingService
    {
        public Task<Dictionary<string, string>> GetSettingsAsync();
        public Task<bool> SaveSettingsAsync(Dictionary<string, string> settings);
    }
}
