﻿namespace Domain.Models;

public class UserModel
{
	public Guid Id { get; set; }
	public string Username { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
}
