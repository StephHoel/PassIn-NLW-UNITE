using Application.UseCases.Attendees;
using Application.UseCases.Events;
using Communication.Requests;
using Communication.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Api.Controllers;

[ExcludeFromCodeCoverage]
[Route("api/[controller]")]
[ApiController]
public class AttendeesController : ControllerBase
{
    [HttpPost]
    [Route("{eventId}/register")]
    [ProducesResponseType(typeof(ResponseRegisteredJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status409Conflict)]
    public IActionResult RegisterAttendeeOnEvent([FromServices] RegisterAttendeeOnEventUseCase useCase, [FromRoute] Guid eventId, [FromBody] RequestRegisterEventJson request)
    {
        var result = useCase.Execute(eventId, request);

        return Created(string.Empty, result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseAllAttendeesJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    [Route("{eventId}")]
    public IActionResult GetAllAttendeesOnAnEvent([FromServices] GetAllByEventIdUseCase useCase, [FromRoute] Guid eventId)
    {
        var response = useCase.Execute(eventId);

        return Ok(response);
    }
}