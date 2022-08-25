using MyAngularApp.Entities;

namespace MyAngularApp.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
