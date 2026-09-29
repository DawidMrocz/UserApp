namespace Gateway.Api.Service
{
    public interface ITokenService
    {
        Dictionary<string, string?> GetToken();
    }
}
