

namespace Repo.Models.Person
{

public class PersonAddModel
{
	public long Id { get; set; }

	public string Fio { get; set; } = string.Empty;
	public string Phone { get; set; } = string.Empty;
    
	public long UserId { get; set; }
	public long CardId { get; set; } 
}
}