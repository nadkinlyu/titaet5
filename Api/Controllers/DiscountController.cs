using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublishingHouse.Interfaces.Extensions.Pagination;
using PublishingHouse.Models.Faculty;
using Repo.Interfaces;
using Repo.Models.Discont;
using Service.Model;

namespace Api.Controllers;

/// <summary>
///     Факультеты
/// </summary>
[Route("/[Controller]")]
[Produces("application/json")]
public class DiscountController : Microsoft.AspNetCore.Mvc.Controller
{
	private readonly IDiscountService _discount;

	public DiscountController(IDiscountService discount)
	{
		_discount = discount;
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
	//[Authorize]
	public async Task<BaseResponse<long>> Add([FromBody] AddDiscountRequest model)
	{
		var result = await _discount.Add(model.Id,model.name,model.value);
		return new BaseResponse<long>(result?.Id ?? 0);
	}

	/// <summary>
	///     Получить список всех факультетов
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	[Route($"{nameof(GetAll)}")]
	[ProducesResponseType(200, Type = typeof(BaseResponse<GetDiscountResponse>))]
	[ProducesResponseType(400, Type = typeof(BaseResponse))]
	public async Task<BaseResponse<GetDiscountResponse>> GetAll([FromQuery] Page request)
	{
		var result = await _discount.GetAllDiscountAsync(new GetDiscountRequest {Page = request});
		return new BaseResponse<GetDiscountResponse>(result);
	}

	/// <summary>
	///     Получить факультет
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	[Route($"{nameof(Get)}")]
	[ProducesResponseType(200, Type = typeof(BaseResponse<(long Id, string Name,double Value)>))]
	[ProducesResponseType(400, Type = typeof(BaseResponse))]
	public async Task<BaseResponse<(long Id, string Name,double Value)>> Get([FromQuery] [Required] long id)
	{
		var result = await _discount.GetDiscountAsync(id);
		return new BaseResponse<(long Id, string Name,double Value)>((result.Id, result.name, result.Value));
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
	public async Task<BaseResponse> Rename([FromQuery] long id,  string name,double value)
	{
		await _discount.Update(id, name,value);
		return new BaseResponse();
	}

	/// <summary>
	///     Удалить факультет
	/// </summary>
	/// </summary>
	/// <returns></returns>
	[HttpDelete]
	[Route($"{nameof(Delete)}")]
	[ProducesResponseType(200, Type = typeof(BaseResponse))]
	[ProducesResponseType(400, Type = typeof(BaseResponse))]
	//[Authorize]
	public async Task<BaseResponse> Delete([FromQuery] long id)
	{
		await _discount.DeleteDiscountAsync(id);
		return new BaseResponse();
	}
}