using Application.Services.Auth;
using Application.Services.User;
using Domain.Context;
using Domain.Repositories.Auth;
using Domain.Repositories.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;
using WebUi.Middlewares;

namespace WebUi.Configuration;

public static class DependencyInjection
{
	public static IServiceCollection AddApplicationDependencyInjection(this IServiceCollection services)
	{
		services.AddScoped<IUserRepository, UserRepository>();
		services.AddScoped<IAuthRepository, AuthRepository>();

		services.AddScoped<IUserService, UserService>();
		services.AddScoped<IAuthService, AuthService>();

		services.AddTransient<GlobalExceptionHandlingMiddleware>();

		return services;
	}

	public static IServiceCollection AddApplicationDbContext(this IServiceCollection services,
		IConfiguration config)
	{
		services.AddDbContext<ApplicationDbContext>(options =>
		{
			options.UseSqlServer(config.GetConnectionString("NetCoreDatabase"), 
				assembly => assembly.MigrationsAssembly(typeof(ApplicationDbContext)
				.Assembly.FullName));
		});

		return services;
	}

	public static IServiceCollection AddAuthenticationAndAuthorization(this IServiceCollection services,
		IConfiguration config)
	{
		services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = config["Jwt:Issuer"],
			ValidAudience = config["Jwt:Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:key"]!))
		};
	});

		services.AddAuthorization();

		return services;
	}

	public static IServiceCollection AddRedisCaching(this IServiceCollection services,
		IConfiguration  config)
	{
		services.AddMemoryCache();

		services.AddStackExchangeRedisCache(options =>
		{
			options.Configuration = config.GetConnectionString("Redis");
			options.InstanceName = "NetCoreApp_";
		});

		return services;
	}

    public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(policy =>
        {
            policy.AddPolicy("OpenCorsPolicy", opt =>
                opt
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());
        });

        return services;
    }
}
