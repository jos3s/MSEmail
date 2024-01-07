namespace MsEmail.API.Models;

public class ApiResult<T>
{
    public T Data { get; private set; }
    public List<string> Erros { get; private set; } = new();

    public ApiResult()
    {
    }

    public ApiResult(string error)
    {
        Erros.Add(error);
    }

    public ApiResult(List<string> erros)
    {
        Erros = erros;
    }

    public ApiResult(T data)
    {
        Data = data;
    }

}