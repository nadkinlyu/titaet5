using Data.Models;
using PublishingHouse.Interfaces.Model.Author;
using Repo.Models.Person;

namespace Repo.Interfaces;



public interface IPersonService
{
    Task<PersonShort> Add(long id,string fio,string phone,long cardid);

    Task<SearchPersonResponse> SearchPerson(PersonGetModel model);

    Task<GetPersonResponse> GetPersonAsync(GetPersonRequest request);

    Task Update(long id,string fio,string phone,long cardid);

    Task Remove(long id);
}