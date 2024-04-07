using Application.UseCases.Attendees;
using Communication.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Api.Controllers;

[ExcludeFromCodeCoverage]
[Route("api/[controller]")]
[ApiController]
public class CheckInController : ControllerBase
{
    [HttpPost]
    [Route("{attendeeId}")]
    [ProducesResponseType(typeof(ResponseRegisteredJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status409Conflict)]
    public IActionResult CheckInAttendeeOnEvent([FromServices] CheckInAttendeeOnEventUseCase useCase, [FromRoute] Guid attendeeId)
    {
        var result = useCase.Execute(attendeeId);

        return Created(string.Empty, result);
    }
}