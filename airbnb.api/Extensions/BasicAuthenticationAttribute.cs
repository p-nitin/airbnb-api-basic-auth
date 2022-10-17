using Microsoft.AspNetCore.Mvc;

namespace airbnb.api.Extensions
{
    public class BasicAuthenticationAttribute : TypeFilterAttribute
    {
        public BasicAuthenticationAttribute(string realm = @"My Realm") : base(typeof(BasicAuthenticationFilter))
        {
            Arguments = new object[] { realm };
        }
    }
}
