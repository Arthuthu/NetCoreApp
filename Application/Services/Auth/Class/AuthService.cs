﻿using Application.Services.Auth.Interface;
using Domain.Models;
using Domain.Repositories.Auth.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services.Auth.Class;

public class AuthService : IAuthService
{
	private readonly IAuthRepository _authRepository;
	private readonly IConfiguration _config;

	public AuthService(IAuthRepository authRepository, IConfiguration config)
	{
		_authRepository = authRepository;
		_config = config;
	}

	public async Task<string?> Login(UserModel user, CancellationToken cancellationToken)
	{
		UserModel? requestedUser = await _authRepository.Login(user, cancellationToken);

		if (requestedUser is null)
			return null;

		string token = GenerateToken(requestedUser);

		return token;
	}

	private string GenerateToken(UserModel user)
	{
		var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
		var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

		var claims = new[]
		{
			new Claim(ClaimTypes.NameIdentifier, user.Username)
		};

		JwtSecurityToken token = new JwtSecurityToken(_config["Jwt:Issuer"],
			_config["Jwt:Audience"],
			claims,
			expires: DateTime.UtcNow.AddDays(1),
			signingCredentials: credentials);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}
}
