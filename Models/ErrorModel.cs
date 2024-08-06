using MyWebApi.Enums;
namespace MyWebApi.Models;

    public class ErrorModel
    {
       public ErrorType ErrorType {get; set;}
       public string? ErrorCode {get; set;}
       public string? ErrorMessage{get; set;}
       public string? ErrorDetail{get; set;}
       
    
    public ErrorModel(ErrorType errorType,string errorCode,string errorMessage,string errorDetail){
            ErrorType = errorType;
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
            ErrorDetail = errorDetail;
          
    }
    }
