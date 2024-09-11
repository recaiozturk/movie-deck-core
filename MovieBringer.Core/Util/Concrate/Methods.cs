
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MovieBringer.WebApp.Util.Abstract;

namespace MovieBringer.Core.Util.Concrate
{
    public class Methods : IMethods
    {
        public List<string> EnumurableToStringList(IEnumerable<IdentityError> errors)
        {
            List<string> errorMessages = new List<string>();

            foreach (var error in errors)
            {
                errorMessages.Add(error.Description);
            }

            return errorMessages;
        }

        public List<string> ModelErrors(ModelStateDictionary modelState)
        {
            var modelErrors = new List<string>();
            foreach (var mState in modelState.Values)
            {
                foreach (var modelError in mState.Errors)
                {
                    modelErrors.Add(modelError.ErrorMessage);
                }
            }
            return modelErrors;
        }
    }
}
