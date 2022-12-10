using PublishingHouse.Interfaces.Extensions.Pagination;

namespace Repo.Models.Person{

public class GetPersonRequest : IPaginationRequest
{
	public long? Id { get; set; } = null;

	public Page Page { get; set; } = new Page();
}
}