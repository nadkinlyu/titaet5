namespace Service.Model.User
{

public class TokenResponse
{
	public long UserId { get; set; }

	public string Fio { get; set; } = string.Empty;

	public string Token { get; set; } = string.Empty;

}
}