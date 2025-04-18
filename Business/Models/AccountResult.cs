namespace Business.Models;

public class AccountResult : ServiceResult
{
}

public class AccountResul<T> : ServiceResult
{
    public T? Result { get; set; }
}