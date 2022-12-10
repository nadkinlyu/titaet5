using PublishingHouse.Interfaces.Extensions.Pagination;

namespace Repo.Models.Person{

public class PersonGetModel : IPaginationRequest
{
	public string Search { get; set; } = string.Empty;

	public Page Page { get; set; } = new Page();
}
}