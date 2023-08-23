﻿using Application.Services.Auth.Class;
using Application.Services.Auth.Interface;
using Application.Services.User.Class;
using Application.Services.User.Interface;
using Domain.Context;
using Domain.Repositories.Auth.Class;
using Domain.Repositories.Auth.Interface;
using Domain.Repositories.User.Class;
using Domain.Repositories.User.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebUi.Configuration;

public static class DependencyInjection
{
	public static IServiceCollection AddApplicationDependencyInjection(this IServiceCollection services)
	{
		services.AddScoped<IUserRepository, UserRepository>();
		services.AddScoped<IAuthRepository, AuthRepository>();

		services.AddScoped<IUserService, UserService>();
		services.AddScoped<IAuthService, AuthService>();

		return services;
	}

	public static IServiceCollection AddApplicationDbContext(this IServiceCollection services,
		IConfiguration config)
	{
		services.AddDbContext<ApplicationDbContext>(options =>
		{
			options.UseSqlServer(config.GetConnectionString("NetCoreDatabase"));
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
}
