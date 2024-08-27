using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;

public class ErrorHandler
{
    public IActionResult HandleException<T>(Exception exception, HttpStatusCode statusCode, string message)
    {
        // Handle exception logic here
        // You can log the except   ion, format the error    message, etc.
        // Here we're returning the default value of type T for simplicity

        var errorResponse = new { message = message };
        return new ObjectResult(errorResponse) { StatusCode = (int)statusCode };
    }

    public IActionResult HandleUnauthorizedAccessException(UnauthorizedAccessException exception)
    {
        return HandleException<IActionResult>(exception, HttpStatusCode.Unauthorized, "Unauthorized access.");
    }

    public IActionResult HandleInvalidOperationException(InvalidOperationException exception)
    {
        return HandleException<IActionResult>(exception, HttpStatusCode.BadRequest, "Invalid operation.");
    }

    // Add more methods for handling specific exception types as needed
}
