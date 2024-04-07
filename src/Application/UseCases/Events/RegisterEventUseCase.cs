using Communication.Requests;
using Communication.Responses;
using Domain.Interfaces;

namespace Application.UseCases.Events;

public class RegisterEventUseCase
{
    private readonly IEventRepository _respository;

    public RegisterEventUseCase(IEventRepository respository)
    {
        _respository = respository;
    }

    public ResponseRegisteredJson Execute(RequestEventJson request)
    {
        var response = _respository.CreateNewEvent(request);

        return response;
    }
}