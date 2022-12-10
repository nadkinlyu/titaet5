namespace PublishingHouse.Models.Faculty;

/// <summary>
///     Запрос добавления факультета
/// </summary>
public class AddCardRequest
{
	/// <summary>
	///     данные карты
	/// </summary>
	
	public long Id { get; set; }
	
	public long PersonId { get; set; }
   
	
	public long DiscontId { get; set; }
}