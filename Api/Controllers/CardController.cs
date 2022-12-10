using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublishingHouse.Interfaces.Extensions.Pagination;
using PublishingHouse.Models.Faculty;
using Repo.Interfaces;
using Repo.Models.Card;
using Repo.Models.Discont;
using Service.Model;

namespace Api.Controllers;

/// <summary>
///     Факультеты
/// </summary>
[Route("/[Controller]")]
[Produces("application/json")]
public class CardController : Microsoft.AspNetCore.Mvc.Controller
{
	private readonly ICardService _card;

	public CardController(ICardService card)
	{
		_card = card;
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
	public async Task<BaseResponse<long>> Add([FromBody] AddCardRequest model)
	{
		var result = await _card.Add(model.Id,model.PersonId,model.DiscontId);
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
	public async Task<BaseResponse<SearchCardResponse>> Search([FromQuery] CardGetModel model)
	{
		var result = await _card.SearchCard(model);
		return new BaseResponse<SearchCardResponse>(result);
	}
	/// <summary>
	///     Получить факультет
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	[Route($"{nameof(Get)}")]
	[ProducesResponseType(200, Type = typeof(BaseResponse<GetCardResponse>))]
	[ProducesResponseType(400, Type = typeof(BaseResponse))]
	public async Task<BaseResponse<GetCardResponse>> Get([FromQuery] GetCardRequest request)
	{
		var result = await _card.GetCardAsync(request);
		return new BaseResponse<GetCardResponse>(result);
	}

	/// <summary>
	///     Переименовать факультет
	/// </summary>
	/// <returns></returns>
	[HttpPatch]
	[Route($"{nameof(Rename)}")]
	[ProducesResponseType(200, Type = typeof(BaseResponse))]
	[ProducesResponseType(400, Type = typeof(BaseResponse))]
	[Authorize]
	public async Task<BaseResponse> Rename([FromQuery] long id, [FromQuery] long personid,[FromQuery] long discountid)
	{
		await _card.Update(id, personid,discountid);
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
		await _card.DeleteCardAsync(id);
		return new BaseResponse();
	}
}