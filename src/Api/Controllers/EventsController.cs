using Application.UseCases.Events;
using Communication.Requests;
using Communication.Responses;
using Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventsController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredEventJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public IActionResult Register([FromBody] RequestEventJson request)
    {
        try
        {
            var useCase = new RegisterEventUseCase();

            var result = useCase.Execute(request);

            return Created(string.Empty, result);
        }
        catch (PassInException ex)
        {
            return BadRequest(new ResponseErrorJson(ex.Message));
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorJson("Unknown error"));
        }
    }

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseEventJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public IActionResult GetById([FromRoute] Guid id)
    {
        try
        {
            var useCase = new GetEventByIdUseCase();

            var response = useCase.Execute(id);

            return Ok(response);
        }
        catch (PassInException ex)
        {
            return NotFound(new ResponseErrorJson(ex.Message));
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorJson("Unknown error"));
        }
    }
}