using PublishingHouse.Interfaces.Extensions.Pagination;
using Repo.Models.Discont;

namespace Repo.Models.Card;

public class SearchCardResponse: IPaginationResponse<CardShortModel>
{
    public Page Page { get; set; } = new Page();

    public long Count { get; set; }

    public IReadOnlyCollection<CardShortModel> Items { get; set; }
}