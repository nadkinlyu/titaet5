using Data.Models;
using Repo.Models.Card;

namespace Repo.Interfaces;

public interface ICardService
{
    Task<Card> Add(long id, long userid, long discontid);

    Task<SearchCardResponse> SearchCard(CardGetModel model);

    Task<GetCardResponse> GetCardAsync(GetCardRequest request);

    Task Update(long id, long userid, long discontid);
    Task DeleteCardAsync(long id);

}