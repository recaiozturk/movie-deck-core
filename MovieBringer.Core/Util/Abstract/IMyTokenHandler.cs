
using MovieBringer.Core.Entities;


namespace MovieBringer.WebApp.Util
{
    public interface IMyTokenHandler
    {
        object GenerateToken(AppUser user);
    }
}
