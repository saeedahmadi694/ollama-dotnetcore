using Microsoft.AspNetCore.Authentication.Cookies;

namespace RAG.AI.Presenter.Web.DependencyInjection;

public static class AddConfiguredCookieAuthenticationInjection
{
    public static IServiceCollection AddConfiguredCookieAuthentication(this IServiceCollection services)
    {

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        }).AddCookie(options =>
        {
            options.LoginPath = "/Login";
            options.LogoutPath = "/Logout";
            options.ExpireTimeSpan = TimeSpan.FromMinutes(43200);
        });

        return services;
    }
}


