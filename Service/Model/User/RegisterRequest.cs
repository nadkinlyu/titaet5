﻿using System.ComponentModel.DataAnnotations;

namespace Service.Model.User{

public class RegisterRequest
{
	[Required]
	public string Email { get; set; } = string.Empty;

	[Required]
	public string Password { get; set; } = string.Empty;

	[Required]
	public string ConfirmPassword { get; set; } = string.Empty;

	public string Fio { get; set; } = string.Empty;

	
}
}