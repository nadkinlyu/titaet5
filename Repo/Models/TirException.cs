using Repo.Enums;
namespace Repo.Models;


public class TirException:Exception
{ 
    public EnumErrorCode ErrorCode { get; set; }
    
    public TirException(string? message = null, EnumErrorCode errorCode = EnumErrorCode.Unknown) : base(message ?? errorCode.GetDescription())
    {
        ErrorCode = errorCode;
    }

    public TirException(EnumErrorCode errorCode) : base(errorCode.GetDescription())
    {
        ErrorCode = errorCode;
    }

   
}