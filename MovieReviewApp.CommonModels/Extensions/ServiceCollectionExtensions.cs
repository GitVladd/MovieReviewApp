using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

public static class ServiceCollectionExtensions
{
	public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
	{
		var issuer = configuration["Jwt:Issuer"];
		var audience = configuration["Jwt:Audience"];
		var key = configuration["Jwt:Key"];

		if (string.IsNullOrWhiteSpace(issuer))
		{
			throw new ArgumentException("JWT:Issuer is not configured.");
		}

		if (string.IsNullOrWhiteSpace(audience))
		{
			throw new ArgumentException("JWT:Audience is not configured.");
		}

		if (string.IsNullOrWhiteSpace(key))
		{
			throw new ArgumentException("JWT:Key is not configured.");
		}

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