using Communication.Requests;
using Communication.Responses;
using Domain.Interfaces;

namespace Application.UseCases.Events;

public class RegisterAttendeeOnEventUseCase
{
    private readonly IAttendeeRepository _repository;

    public RegisterAttendeeOnEventUseCase(IAttendeeRepository repository)
    {
        _repository = repository;
    }

    public ResponseRegisteredJson Execute(Guid eventId, RequestRegisterEventJson request)
    {
        var response = _repository.CreateNewAttendee(eventId, request);

        return response;
    }
}