using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RAG.AI.Infrastructure.Configurations;
using System.Text;

namespace RAG.AI.Presenter.API.DependencyInjection;
public static class AddAuthenticationInjection
{
    public static IServiceCollection AddTokenAuthentication(this IServiceCollection services, IConfiguration config)
    {

        JwtConfig siteSettings = new();

        config.GetSection(JwtConfig.Key).Bind(siteSettings);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            //options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidAudience = siteSettings.ValidAudience,
                ValidIssuer = siteSettings.ValidIssuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(siteSettings.Secret))
            };
        });

        _ = services.AddAuthorization();

        return services;
    }
}


