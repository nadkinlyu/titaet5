using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublishingHouse.Interfaces.Extensions.Pagination;
using PublishingHouse.Models.Faculty;
using Repo.Interfaces;
using Repo.Models.Card;
using Repo.Models.Discont;
using Repo.Models.Person;
using Service.Model;

namespace Api.Controllers;

/// <summary>
///     Факультеты
/// </summary>
[Route("/[Controller]")]
[Produces("application/json")]
public class PersonController : Microsoft.AspNetCore.Mvc.Controller
{
	private readonly IPersonService _person;

	public PersonController(IPersonService person)
	{
		_person = person;
	}

	/// <summary>
	///     Добавить дисконт
	/// </summary>
	/// <param name="model"></param>
	/// <returns></returns>
	[HttpPost]
	[Route($"{nameof(Add)}")]
	[ProducesResponseType(200, Type = typeof(BaseResponse<long>))]
	[ProducesResponseType(400, Type = typeof(BaseResponse))]
	[Authorize]
	public async Task<BaseResponse<long>> Add([FromBody] PersonAddModel model)
	{
		var result = await _person.Add(model.Id,model.Fio,model.Phone,model.CardId);
		return new BaseResponse<long>(result?.Id ?? 0);
	}

	/// <summary>
	///     Получить список всех факультетов
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	[Route($"{nameof(Search)}")]
	[ProducesResponseType(200, Type = typeof(BaseResponse<SearchCardResponse>))]
	[ProducesResponseType(400, Type = typeof(BaseResponse))]
	public async Task<BaseResponse<SearchPersonResponse>> Search([FromQuery] PersonGetModel model)
	{
		var result = await _person.SearchPerson(model);
		return new BaseResponse<SearchPersonResponse>(result);
	}
	/// <summary>
	///     Получить персону
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	[Route($"{nameof(Get)}")]
	[ProducesResponseType(200, Type = typeof(BaseResponse<GetCardResponse>))]
	[ProducesResponseType(400, Type = typeof(BaseResponse))]
	public async Task<BaseResponse<GetPersonResponse>> Get([FromQuery] GetPersonRequest request)
	{
		var result = await _person.GetPersonAsync(request);
		return new BaseResponse<GetPersonResponse>(result);
	}

	/// <summary>
	///     Переименовать gthcjye
	/// </summary>
	/// <returns></returns>
	[HttpPatch]
	[Route($"{nameof(Rename)}")]
	[ProducesResponseType(200, Type = typeof(BaseResponse))]
	[ProducesResponseType(400, Type = typeof(BaseResponse))]
	[Authorize]
	public async Task<BaseResponse> Rename([FromQuery] long id, string fio,string phone,[FromQuery] long cardid)
	{
		await _person.Update(id, fio,phone,cardid);
		return new BaseResponse();
	}

	/// <summary>
	///     Удалить факультет
	/// </summary>
	/// <returns></returns>
	[HttpDelete]
	[Route($"{nameof(Delete)}")]
	[ProducesResponseType(200, Type = typeof(BaseResponse))]
	[ProducesResponseType(400, Type = typeof(BaseResponse))]
	[Authorize]
	public async Task<BaseResponse> Delete([FromQuery] long id)
	{
		await _person.Remove(id);
		return new BaseResponse();
	}
}