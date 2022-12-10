using Data.Models;
using Repo.Models.Discont;

namespace Repo.Interfaces;

public interface IDiscountService
{
    Task<Discount> Add(string name, double value);

    Task<GetDiscountResponse> GetAllDiscountAsync(GetDiscountRequest model);

    Task<Discount> GetDiscountAsync(long discountId);

    Task Update(long id, string name, double value);
    Task DeleteDiscountAsync(long id);
}