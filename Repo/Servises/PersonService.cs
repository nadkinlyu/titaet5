using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using PublishingHouse.Interfaces.Extensions.Pagination;
using PublishingHouse.Interfaces.Model.Author;
using Repo.Enums;
using Repo.Interfaces;
using Repo.Models;
using Repo.Models.Person;
using Repo.Servises;

namespace Repo.Services;

public class PersonService : IPersonService
{
	private readonly AplicationContext _db;

	public PersonService(AplicationContext db)
	{
		_db = db;
	}

	public async Task<PersonShort> Add(long id,string fio,string phone,long cardid)
	{
		if (await _db.Users.AllAsync(x => x.Id != id))
			throw new TirException($"User {id} is not exists!", EnumErrorCode.EntityIsNotFound);
		
		var person = new Person
		{
			
			Id = id,
			Fio = fio,
			Phone = phone,
			UserId = id,
			CardId = cardid
			
		};		
		await _db.AddAsync(person);
		await _db.SaveChangesAsync();

		return new PersonShort
		{
	 Id =person.Id,

	 Fio =person.Fio,
	Phone=person.Phone,
	CardId = person.CardId,
		};
	}

	public async Task<SearchPersonResponse> SearchPerson(PersonGetModel model)
	{
		return await _db.Persons
			.Where(x =>
				x.Fio.Contains(model.Search)
				|| x.Phone.Contains(model.Search)
				).GetPageAsync<SearchPersonResponse, Person, PersonShort>(model, x => new PersonShort
			{
				Id =x.Id,
				Fio =x.Fio,
				Phone=x.Phone,
				CardId = x.CardId,
			});
	}


	public async Task<GetPersonResponse> GetPersonAsync(GetPersonRequest request)
	{
		return await _db.Persons.GetPageAsync<GetPersonResponse, Person, PersonShort>(request, person =>
			new PersonShort
			{
				Id = person.Id,
				Fio = person.Fio,
				Phone = person.Phone,
				CardId = person.CardId,
				
			});
		
	}

	public async Task Update(long id,string fio,string phone,long cardid)
	{
		var person = await _db.Persons.FirstOrDefaultAsync(x => x.Id == id);
		if (person is null)
			throw new TirException($"Person Id = {id} is not found!", EnumErrorCode.EntityIsNotFound);

		if (!string.IsNullOrWhiteSpace(fio))
			person.Fio = fio;

		if (!string.IsNullOrWhiteSpace(phone))
			person.Phone = phone;
		if (cardid > 0)
		{
			if (await _db.Cards.AllAsync(x => x.Id != cardid))
				throw new TirException($"Cart {cardid} is not exists!", EnumErrorCode.EntityIsNotFound);
			person.CardId = cardid;
		}
		await _db.SaveChangesAsync();
	}

	public async Task Remove(long id)
	{
		if (await _db.Persons.AllAsync(x => x.Id != id))
			throw new TirException($"Person id = {id} is not exists!",EnumErrorCode.EntityIsNotFound);

	
		_db.Persons.Remove(new Person { Id = id });
		await _db.SaveChangesAsync();
	}
}
