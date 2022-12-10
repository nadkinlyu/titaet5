using PublishingHouse.Interfaces.Extensions.Pagination;

namespace Repo.Models.Card;

public class CardGetModel: IPaginationRequest
{
    public string Search { get; set; } = string.Empty;

    public Page Page { get; set; } = new Page();
}