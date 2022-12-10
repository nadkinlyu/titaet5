using Service.Model;
using Service.Model.User;

namespace Service
{

	public interface IUserService
	{
		Task<BaseResponse<UserResponse>> Login(UserRequest request);

		Task<BaseResponse> Register(RegisterRequest request);

		Task<BaseResponse<TokenResponse>> ActivateAccount(ActivateAccountRequest request);
	}
}