using Data.Models;
using Repo.Models.User;

namespace Repo.Interfaces;

public interface IUserRepo
{
    Task<User> Add(string login, string fio,  string email, string pas1,string pas2);

    Task<SearchUserResponse> SearchUser(UserGetModel model);

    Task Update(long id, string fio, string pas1,string pas2);
    Task DeleteUserAsync(long id);

}