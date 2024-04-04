using Communication.Responses;
using Domain.Interfaces;

namespace Application.UseCases.Attendees;

public class CheckInAttendeeOnEventUseCase
{
    private readonly ICheckInRepository _repository;

    public CheckInAttendeeOnEventUseCase(ICheckInRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResponseRegisteredJson> Execute(Guid attendeeId)
    {
        var response = _repository.DoCheckIn(attendeeId);

        return response;
    }
}