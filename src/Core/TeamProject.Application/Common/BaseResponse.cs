using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject.Application.Common;

public class BaseResponse
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }

    public static BaseResponse Success(string message = "Operation successful")
        => new() { IsSuccess = true, Message = message };

    public static BaseResponse Fail(string message)
        => new() { IsSuccess = false, Message = message };
}

public class BaseResponse<T> : BaseResponse
{
    public T? Data { get; set; }

    public static BaseResponse<T> Success(T data, string message = "Operation successful")
        => new() { IsSuccess = true, Data = data, Message = message };

    public new static BaseResponse<T> Fail(string message)
        => new() { IsSuccess = false, Message = message };
}

