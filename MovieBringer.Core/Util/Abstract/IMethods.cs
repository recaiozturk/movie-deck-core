using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace MovieBringer.WebApp.Util.Abstract
{
    public interface IMethods
    {
        List<string> EnumurableToStringList(IEnumerable<IdentityError> errors);
        List<string> ModelErrors(ModelStateDictionary modelState);
    }
}
