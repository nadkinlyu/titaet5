using Service.Model;
using Service.Model.User;

namespace Service;

public interface IOrder
{
    Task<BaseResponse> AddOrder(RegisterRequest request);
}