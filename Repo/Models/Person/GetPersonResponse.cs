using PublishingHouse.Interfaces.Extensions.Pagination;
using System.Collections.Generic;
using Data.Models;

namespace Repo.Models.Person
{

public class GetPersonResponse : IPaginationResponse<PersonShort>
{
	public Page Page { get; set; } = new Page();

	public long Count { get; set; }

	public IReadOnlyCollection<PersonShort> Items { get; set; }
}
}