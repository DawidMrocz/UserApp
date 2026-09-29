
namespace Common.Services.Setting
{
    internal interface ISettingService
    {
        Task<string> GetByKey(string key);
    }
}