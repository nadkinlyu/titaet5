using PublishingHouse.Interfaces.Extensions.Pagination;

namespace Repo.Models.User{

public class UserGetModel : IPaginationRequest
{
	public string Search { get; set; } = string.Empty;

	public Page Page { get; set; } = new Page();
}
}