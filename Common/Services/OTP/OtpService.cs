using Common.Models.OTP;
using Common.Repositories.OTP;
using Common.Services.Database;

namespace Common.Services.OTP
{
    internal class OtpService : IOtpService
    {
        private readonly IDatabaseService _databaseService;

        public OtpService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<OneTimePasscodeModel?> GetById(int id)
        {
            return await _databaseService.Get<OneTimePasscodeModel>(Sql.User_OTP_GetById, parameters: new { Id = id });
        }

        public async Task<OneTimePasscodeModel?> GetByGuid(Guid guid)
        {
            return await _databaseService.Get<OneTimePasscodeModel>(Sql.User_OTP_GetByGuid, parameters: new { Guid = guid });
        }

        public async Task<int> Create(OneTimePasscodeModel request)
        {
            return await _databaseService.GetValue<int>(Sql.User_OTP_Create, parameters: request);
        }

        public async Task Delete(int id)
        {
            await _databaseService.Delete(Sql.User_OTP_Delete, parameters: new { Id = id });
        }
    }
}
