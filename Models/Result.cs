
namespace MyWebApi.Models;
public class Result{
    public bool isSuccess {get; set;}
    public string? Message{get; set;}
    public ErrorModel? error{get; set;}
    public object? Data{get; set;}

}