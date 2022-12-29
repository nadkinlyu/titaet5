using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Data;
using Data.Enums;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repo.Enums;
using Repo.Models;
using Repo.Models.User;
using Service.Model;
using Service.Model.User;

namespace Service;

public class UserService : IUserService
{
	private readonly AplicationContext _db;
	private readonly MailService _mailService = new();

	public UserService(AplicationContext db)
	{
		_db = db;
	}

	public async Task<BaseResponse<UserResponse>> Login(UserRequest request)
	{
		if (string.IsNullOrWhiteSpace(request.Password))
			throw new TirException(EnumErrorCode.PasswordIsAreRequired);

		if (string.IsNullOrWhiteSpace(request.Email))
			throw new TirException(EnumErrorCode.EmailAreRequired);

		var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == request.Email.ToLower());
		if (user == null )
			
			throw new TirException("нету мыла",EnumErrorCode.AccessDenied);
		if (user.Status is EnumUserStatus.New or EnumUserStatus.Blocked)
			throw new TirException("новый",EnumErrorCode.AccessDenied);
		if (user.PasswordHash != PasswordHashService.GetHashPassword(request.Password, user.PasswordKey))
			throw new TirException("Password are incorrect", EnumErrorCode.AccessDenied);

		var response = new UserResponse
		{
			Token = (await GenerateToken(user.Id)).Token,
			Id = user.Id,
			Fio = user.Fio,
			Role = user.Role
		};

		return new BaseResponse<UserResponse>(response);
	}

	public async Task<BaseResponse> Register(RegisterRequest request)
	{
		if (await _db.Users.AnyAsync(x => x.Email == request.Email))
			throw new TirException(EnumErrorCode.MailIsAlreadyInUse);
////н
		if (string.IsNullOrWhiteSpace(request.Email))
			throw new TirException(EnumErrorCode.EmailAreRequired);

		if (string.IsNullOrWhiteSpace(request.Password))
			throw new TirException(EnumErrorCode.PasswordIsAreRequired);

		if (string.IsNullOrWhiteSpace(request.ConfirmPassword))
			throw new TirException(EnumErrorCode.PasswordIsAreRequired);

		if (request.Password != request.ConfirmPassword)
			throw new TirException(EnumErrorCode.PasswordsDoNotMatch);

		var guidEmail = Guid.NewGuid();
		var password = PasswordHashService.GenHashPassword(request.Password);

		var user = new User
		{
			Email = request.Email,
			Fio = request.Fio,
				Status = EnumUserStatus.New,
			Role = EnumUserRole.User,
			PasswordKey = password.Key,
			PasswordHash = password.Hash
		};

		var token = new MailToken
		{
			DateExpire = DateTime.UtcNow.AddDays(5),
			Key = guidEmail,
			User = user
		};

		await _mailService.RegisterSuccess(new SendRegisterMail
		{
			Email = request.Email,
			Token = guidEmail.ToString("D")
		});

		await _db.MailTokens.AddAsync(token);
		await _db.SaveChangesAsync();

		return new BaseResponse();
	}

	public async Task<BaseResponse<TokenResponse>> ActivateAccount(ActivateAccountRequest request)
	{
		var dateExpire = DateTime.UtcNow;
		var user = await _db.Users.FirstOrDefaultAsync(x =>
			x.Tokens.Any(z => z.Key == request.Key && z.DateExpire > dateExpire));

		if (user is null)
			throw new TirException(EnumErrorCode.TokenIsNotFound);

		user.Status = EnumUserStatus.Verified;
		await _db.SaveChangesAsync();

		return new BaseResponse<TokenResponse>(await GenerateToken(user.Id));
	}

	private async Task<TokenResponse> GenerateToken(long id)
	{
		(User user, ClaimsIdentity claim) identity = await GetIdentity(id);
		if (identity.user is null)
			throw new TirException("User is not found!", EnumErrorCode.EntityIsNotFound);

		var now = DateTime.UtcNow;
		var jwt = new JwtSecurityToken(
			AuthOptions.ISSUER,
			AuthOptions.AUDIENCE,
			notBefore: now,
			claims: identity.claim.Claims,
			expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
			signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
				SecurityAlgorithms.HmacSha256));

		var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

		return new TokenResponse
		{
			UserId = identity.user.Id,
			Fio = identity.user.Fio,

			Token = $"Bearer {encodedJwt}"
		};
	}

	private async Task<(User, ClaimsIdentity)> GetIdentity(long id)
	{
		var person = await _db.Users.FindAsync(id);
		if (person is null)
			throw new TirException("User is not found!", EnumErrorCode.EntityIsNotFound);

		if (person.Status == EnumUserStatus.Blocked)
			throw new TirException(EnumErrorCode.UserIsBlocked);

		var claims = new List<Claim> { new(ClaimTypes.Name, person.Id.ToString()) };
		var claimsIdentity =
			new ClaimsIdentity(claims, "TokenResponse", ClaimsIdentity.DefaultNameClaimType,
				ClaimsIdentity.DefaultRoleClaimType);
		return (person, claimsIdentity);
	}
}