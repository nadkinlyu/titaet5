using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using PublishingHouse.Interfaces.Extensions.Pagination;
using Repo.Enums;
using Repo.Interfaces;
using Repo.Models;
using Repo.Models.Discont;

namespace Repo.Servises;

public class DiscountService : IDiscountService
{
	private readonly AplicationContext _db;

	public DiscountService(AplicationContext db)
	{
		_db = db;
	}
	public async Task<Discount> Add(string name, double value )
	{
		if (await _db.Discounts.AllAsync(x => x.Value == value))
			throw new TirException($"Value {value} already exists!", EnumErrorCode.EntityIsAlreadyExists);
		if (await _db.Discounts.AllAsync(x => x.name == name))
			throw new TirException($"Name {name} already exists!", EnumErrorCode.EntityIsAlreadyExists);
		var discont = new Discount
		{
			name = name,
			Value = value
		};
		await _db.AddAsync(discont);
		await _db.SaveChangesAsync();
		
		return discont;

	}
	public async Task<GetDiscountResponse> GetAllDiscountAsync(GetDiscountRequest request)
	{
		return await _db.Discounts.GetPageAsync<GetDiscountResponse, Discount, DiscountShortModel>(request, discount =>
			new DiscountShortModel
			{
				Id = discount.Id,
				Value = discount.Value,
				name = discount.name
			});
	}
	public async Task<Discount> GetDiscountAsync(long discountId)
	{
		return await _db.Discounts.FirstOrDefaultAsync(x => x.Id == discountId)
		       ?? throw new TirException($"Discount {discountId} is not found!",
			       EnumErrorCode.EntityIsNotFound);
		
	}
	public async Task Update(long id, string name, double value)
	{
		var discount = await _db.Discounts.FirstOrDefaultAsync(x => x.Id == id);
		if (discount is null)
			throw new TirException($"Discount Id = {id} is not found!", EnumErrorCode.EntityIsNotFound);
		if (await _db.Discounts.AllAsync(x => x.Value == value))
			throw new TirException($"Value {value} already exists!", EnumErrorCode.EntityIsAlreadyExists);
		
		if (!string.IsNullOrWhiteSpace(name))
			discount.name = name;

		if (value > 0)
		{
			discount.Value = value;	
			
		}
	
		await _db.SaveChangesAsync();
	}
	public async Task DeleteDiscountAsync(long id)
	{
		if (await _db.Discounts.AnyAsync(x => x.Id == id))
			throw new TirException("Discount is not exists!", EnumErrorCode.EntityIsNotFound);

		_db.Discounts.Remove(new Discount {Id = id});
		await _db.SaveChangesAsync();
	}
}
