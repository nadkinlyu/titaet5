using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using PublishingHouse.Interfaces.Extensions.Pagination;
using Repo.Enums;
using Repo.Interfaces;
using Repo.Models;
using Repo.Models.Card;

namespace Repo.Servises;

public class CardService : ICardService
{
	private readonly AplicationContext _db;

	public CardService(AplicationContext db)
	{
		_db = db;
	}

	public async Task<Card> Add(long id, long personid, long discontid)
	{
		if (await _db.Persons.AllAsync(x => x.Id != personid))
			throw new TirException($"Userid {personid} is not exists!", EnumErrorCode.EntityIsNotFound);

		if (await _db.Discounts.AllAsync(x => x.Id != discontid))
			throw new TirException($"discount id {discontid} already exists!", EnumErrorCode.EntityIsAlreadyExists);
		
		var card = new Card
		{
			Id= id,
			DiscontId= discontid,
			PersonId= personid,
		};
		await _db.AddAsync(card);
		await _db.SaveChangesAsync();
		
		return card;

	}

	public async Task<SearchCardResponse> SearchCard(CardGetModel model)
	{
		return await _db.Cards
			.Where(x =>
				x.PersonId.ToString().Contains(model.Search)
				|| (x.Id.ToString()).Contains(model.Search)
				).GetPageAsync<SearchCardResponse, Card, CardShortModel>(model, x => new CardShortModel
			{
				Id =x.Id,
				DiscontId = x.DiscontId,
				PersonId = x.PersonId,

			
			});
	}



	public async Task<GetCardResponse> GetCardAsync(GetCardRequest request)
	{
		return await _db.Cards.GetPageAsync<GetCardResponse, Card, CardShortModel>(request, card =>
			new CardShortModel
			{Id =card.Id,
				DiscontId =card.DiscontId,
				PersonId = card.PersonId,
			});
		
	}

	
	public async Task Update(long id, long personid, long discontid)
	{
		var card = await _db.Cards.FirstOrDefaultAsync(x => x.PersonId != personid);
		if (card is null)
			throw new TirException($" Id = {id} is not found!", EnumErrorCode.EntityIsNotFound);
		if (await _db.Discounts.AllAsync(x => x.Id != discontid))
			throw new TirException($"Value {discontid} already exists!",  EnumErrorCode.EntityIsNotFound);
		
		if (id!=0)
			card.Id = id;

		if (discontid > 0)
		{
			card.DiscontId = discontid;	
			
		}
		await _db.SaveChangesAsync();
	}


	public async Task DeleteCardAsync(long personid)
	{
		if (await _db.Cards.AnyAsync(x => x.DiscontId == personid))
			throw new TirException("Person is not exists!", EnumErrorCode.EntityIsNotFound);

		_db.Cards.Remove(new Card {PersonId = personid});
		await _db.SaveChangesAsync();
	}


}
