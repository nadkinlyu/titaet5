using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using PublishingHouse.Interfaces.Extensions.Pagination;
using Repo.Enums;
using Repo.Interfaces;
using Repo.Models;
using Repo.Models.Discont;
using Repo.Models.User;

namespace Repo.Servises;

public class UserRepo : IUserRepo
{
	private readonly AplicationContext _db;

	public UserRepo(AplicationContext db)
	{
		_db = db;
	}

	public async Task<User> Add(string login, string fio,  string email, string pas1,string pas2)
	{
		if (await _db.Users.AllAsync(x => x.Login==login))
			throw new TirException($"Value {login} already exists!", EnumErrorCode.EntityIsAlreadyExists);
		if (await _db.Users.AllAsync(x => x.Email == email))
			throw new TirException($"Emayl {email} already exists!", EnumErrorCode.EntityIsAlreadyExists);
		var user = new User
		{
			Login = login,
			Fio = fio, 
			Email= email,
			PasswordHash = pas1,
			PasswordKey = pas2
			
		};
		await _db.AddAsync(user);
		await _db.SaveChangesAsync();
		
		return user;

	}

	

	public async Task<SearchUserResponse> SearchUser(UserGetModel model)
	{
		
		return await _db.Users
			.Where(x =>
				x.Login.Contains(model.Search)
				|| x.Email.Contains(model.Search)
				).GetPageAsync<SearchUserResponse, User, UserShortModel>(model, x => new()
			{
				Id =x.Id,
				Fio = x.Fio,
				Email = x.Email,
				Role = x.Role,
				PasswordHash = x.PasswordHash,
				PasswordKey = x.PasswordKey

			
			});
	}



	
	
	public async Task Update(long id, string fio, string pas1,string pas2)
	{
		var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
		if (user is null)
			throw new TirException($"Person Id = {id} is not found!", EnumErrorCode.EntityIsNotFound);
			
		if (!string.IsNullOrWhiteSpace(fio))
			user.Fio = fio;
		if (!string.IsNullOrWhiteSpace(pas1))
			user.PasswordHash = pas1;
		if (!string.IsNullOrWhiteSpace(pas2))
			user.PasswordKey = pas2;

		
		//await _db.Update(discont);
		await _db.SaveChangesAsync();
	}

	
	public async Task DeleteUserAsync(long id)
	{
		if (await _db.Users.AnyAsync(x => x.Id == id))
			throw new TirException("User is not exists!", EnumErrorCode.EntityIsNotFound);

		_db.Users.Remove(new User {Id = id});
		await _db.SaveChangesAsync();
	}


}
