using PublishingHouse.Interfaces.Extensions.Pagination;

namespace Repo.Models.User{

public class GetUserRequest : IPaginationRequest
{
	public long? Id { get; set; } = null;

	public Page Page { get; set; } = new Page();
}
}