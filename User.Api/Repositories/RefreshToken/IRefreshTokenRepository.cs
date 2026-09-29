using User.Api.Dto.RefreshToken;

namespace User.Api.Repositories.RefreshToken
{
    public interface IRefreshTokenRepository
    {
        Task Create(CreateRefreshToken dto);
        Task Delete(int refreshTokenId);
        Task<GetRefreshTokenDto?> GetByValue(string refreshToken);
        Task<GetRefreshTokenDto?> GetByJwtToken(string jwtToken);
    }
}
