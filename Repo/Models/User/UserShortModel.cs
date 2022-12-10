using Data.Enums;

namespace Repo.Models.User;

public class UserShortModel
{
    public long Id { get; set; }
    public string Login { get; set; }
    public string Fio { get; set; } 

    public string Email { get; set; } 

    public string PasswordHash { get; set; }

    public string PasswordKey { get; set; } 
    public EnumUserRole Role { get; set; } 

    public EnumUserStatus Status { get; set; } 
}