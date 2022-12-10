using PublishingHouse.Interfaces.Extensions.Pagination;

namespace Repo.Models.User;

public class SearchUserResponse : IPaginationResponse<UserShortModel>
{
    public Page Page { get; set; } = new Page();

    public long Count { get; set; }

    public IReadOnlyCollection<UserShortModel> Items { get; set; }
}
