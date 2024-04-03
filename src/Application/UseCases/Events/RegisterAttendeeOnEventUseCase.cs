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

    public async Task<ResponseRegisteredJson> Execute(RequestRegisterEventJson request)
    {
        var response = _repository.CreateNewAttendee(request);

        return response;
    }
}