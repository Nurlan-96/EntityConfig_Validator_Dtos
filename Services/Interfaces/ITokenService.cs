using ShopAppAPI.Entities;

namespace ShopAppAPI.Services.Interfaces
{
    public interface ITokenService
    {
        string GetToken(string secretKey, string audience, string issuer, AppUser user, IList<string> roles);
    }
}
