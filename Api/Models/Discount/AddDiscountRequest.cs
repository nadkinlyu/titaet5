namespace PublishingHouse.Models.Faculty;

/// <summary>
///     Запрос добавления факультета
/// </summary>
public class AddDiscountRequest
{
	/// <summary>
	///     Имя дисконта
	/// </summary>
	///
	///
	public long Id { get; set; } 
	public string name { get; set; } = string.Empty;
	public double value { get; set; } 
}