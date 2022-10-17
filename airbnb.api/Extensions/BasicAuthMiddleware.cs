using airbnb.api.DataModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Text;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace airbnb.api.Extensions
{
    /// <summary>
    /// This class is a global middleware that provides authorization to all controller actions. To enable uncomment this line
    /// app.UseMiddleware<BasicAuthMiddleware>("example-realm.com");
    /// in Program.cs and comment the [BasicAuthentication] attribute in the ListingsController
    /// </summary>
    public class BasicAuthMiddleware
    {
        private readonly string authRealm;
        private readonly RequestDelegate nextFunction;
        private IServiceProvider serviceProvider;

        public BasicAuthMiddleware(string realm, RequestDelegate next, IServiceProvider serviceProvider)
        {
            this.authRealm = realm;
            this.nextFunction = next;
            this.serviceProvider = serviceProvider;

            if (string.IsNullOrWhiteSpace(authRealm))
            {
                throw new ArgumentNullException(nameof(realm), @"Please provide a non-empty realm value.");
            }
        }

        public async Task Invoke(HttpContext httpContext)
        {
            string authHeader = httpContext.Request.Headers["Authorization"];
            if (authHeader != null)
            {
                string auth = authHeader.Split(new char[] { ' ' })[1];
                Encoding encoding = Encoding.GetEncoding("UTF-8");
                var usernameAndPassword = encoding.GetString(Convert.FromBase64String(auth));
                string username = usernameAndPassword.Split(new char[] { ':' })[0];
                string password = usernameAndPassword.Split(new char[] { ':' })[1];

                var isAuthorized = await IsAuthorized(username, password);

                if (isAuthorized)
                {
                    await nextFunction(httpContext);
                }
                else
                {
                    ReturnUnauthorizedResult(httpContext);
                }
            }
            else
            {
                httpContext.Response.StatusCode = 401;
                return;
            }
        }

        public async Task<bool> IsAuthorized(string username, string password)
        {
            UserManager<ApplicationUser>? userManager;
            SignInManager<ApplicationUser>? signInManager;

            using (var scope = serviceProvider.CreateScope())
            {
                userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                signInManager = scope.ServiceProvider.GetService<SignInManager<ApplicationUser>>();

                ApplicationUser appUser = await userManager.FindByNameAsync(username);
                if (appUser != null)
                {
                    SignInResult result = await signInManager.PasswordSignInAsync(appUser, password, false, false);
                    if (result.Succeeded)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        
        private void ReturnUnauthorizedResult(HttpContext context)
        {
            // Return 401 and a basic authentication challenge (causes browser to show login dialog)
            context.Response.Headers["WWW-Authenticate"] = $"Basic realm=\"{authRealm}\"";
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
    }
}
