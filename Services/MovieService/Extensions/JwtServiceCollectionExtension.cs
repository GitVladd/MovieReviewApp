using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

public static class JwtServiceCollectionExtension
{
    public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var issuer = configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT:Issuer is not configured");
        var audience = configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT:Audience is not configured");
        var key = configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT:Key is not configured");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(key))
            };
        });

        services.AddAuthorization();
    }
}