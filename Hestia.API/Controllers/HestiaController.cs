﻿using Hestia.Application.Result;
using Microsoft.AspNetCore.Mvc;

namespace Hestia.API.Controllers;

public abstract class HestiaController(ILogger<HestiaController> logger) : ControllerBase
{
    protected readonly ILogger<HestiaController> Logger = logger;
    
    protected IActionResult HandleResult<T>(IResult<T> result)
    {
        return result.Success ? Ok(result.Data) : HandleFailedResult(result);
    }
    
    protected IActionResult HandleFailedResult<T>(IResult<T> result)
    {
        if (result.Success)
        {
            throw new InvalidOperationException("Result was successful");
        }

        if (result.Message is null)
        {
            Logger.LogError("Result failed but no message was provided");
            result.Message = "An unknown error occurred";
        }
        else
        {
            Logger.LogError("Result failed: {Message}", result.Message);
        }
        return BadRequest(result.Message);
    }
}