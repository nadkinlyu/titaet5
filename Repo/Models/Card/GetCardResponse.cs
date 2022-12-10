using PublishingHouse.Interfaces.Extensions.Pagination;

namespace Repo.Models.Card;

public class GetCardResponse: IPaginationResponse<CardShortModel>
{
    public Page Page { get; set; } = new Page();

    public long Count { get; set; }

    public IReadOnlyCollection<CardShortModel> Items { get; set; }
}