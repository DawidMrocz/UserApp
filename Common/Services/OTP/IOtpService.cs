using Common.Models.OTP;

namespace Common.Services.OTP
{
    internal interface IOtpService
    {
        Task<int> Create(OneTimePasscodeModel request);
        Task Delete(int id);
        Task<OneTimePasscodeModel?> GetByGuid(Guid guid);
        Task<OneTimePasscodeModel?> GetById(int id);
    }
}