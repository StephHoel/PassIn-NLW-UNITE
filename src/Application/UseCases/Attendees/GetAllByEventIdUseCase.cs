using Communication.Responses;
using Domain.Interfaces;

namespace Application.UseCases.Attendees;

public class GetAllByEventIdUseCase
{
    private readonly IAttendeeRepository _repository;

    public GetAllByEventIdUseCase(IAttendeeRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResponseAllAttendeesJson> Execute(Guid eventId)
    {
        var response = _repository.GetAllByEventId(eventId);

        return response;
    }
}