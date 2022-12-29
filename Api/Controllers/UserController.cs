using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Model;
using Service.Model.User;

namespace Api.Controllers;

/// <summary>
/// Авторизация
/// </summary>
[Route("/[controller]")]
[Produces("application/json")]
public class AuthController : Microsoft.AspNetCore.Mvc.Controller
{
	[HttpPost]
	[Route($"{nameof(Login)}")]
	[ProducesResponseType(200, Type = typeof(BaseResponse<UserResponse>))]
	[ProducesResponseType(400, Type = typeof(BaseResponse))]
	public async Task<BaseResponse<UserResponse>> Login([FromServices] IUserService auth, [FromQuery] UserRequest request)
	{
		return await auth.Login(request);
	}

	[HttpPut]
	[Route($"{nameof(Register)}")]
	[ProducesResponseType(200, Type = typeof(BaseResponse))]
	[ProducesResponseType(400, Type = typeof(BaseResponse))]
	public async Task<BaseResponse> Register([FromServices] IUserService auth, [FromQuery] RegisterRequest request)
	{
		return await auth.Register(request);
	}

	[HttpPatch]
	[Route($"{nameof(ActivateAccount)}")]
	[ProducesResponseType(200, Type = typeof(BaseResponse<TokenResponse>))]
	[ProducesResponseType(400, Type = typeof(BaseResponse))]
	public async Task<BaseResponse> ActivateAccount([FromServices] IUserService auth, [FromBody] ActivateAccountRequest request)
	{
		return await auth.ActivateAccount(request);
	}
}