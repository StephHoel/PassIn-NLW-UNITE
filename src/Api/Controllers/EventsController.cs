using Application.UseCases.Events;
using Communication.Requests;
using Communication.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Api.Controllers;

[ExcludeFromCodeCoverage]
[Route("api/[controller]")]
[ApiController]
public class EventsController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public IActionResult RegisterEvent([FromServices] RegisterEventUseCase useCase, [FromBody] RequestEventJson request)
    {
        var result = useCase.Execute(request);

        return Created(string.Empty, result);
    }

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseEventJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public IActionResult GetEventById([FromServices] GetEventByIdUseCase useCase, [FromRoute] Guid id)
    {
        var response = useCase.Execute(id);

        return Ok(response);
    }
}