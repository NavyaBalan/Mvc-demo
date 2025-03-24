using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;

[assembly: OwinStartup(typeof(MvcDemo.Startup))]

namespace MvcDemo
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ApplicationCookie",
                LoginPath = new PathString("/Account/Login")
            });

            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions
            {
                ClientId = "806474400503-biff7704ud1k9d74j3h11n588ql8cd8t.apps.googleusercontent.com",
                ClientSecret = "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",
                CallbackPath = new PathString("/signin-google")
            });
        }
    }
}
