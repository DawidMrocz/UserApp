using Common.Repositories.Setting;
using Common.Services.Database;

namespace Common.Services.Setting
{
    internal class SettingService : ISettingService
    {
        private readonly IDatabaseService _databaseService;

        public SettingService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<string> GetByKey(string key)
        {
            return await _databaseService.Get<string>(Sql.Setting_Setting_Get, parameters: new { Key = key })
                ?? throw new Exception("Nie znaleziono klucza");
        }
    }
}
