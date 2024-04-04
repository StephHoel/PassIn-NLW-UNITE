using Communication.Responses;

namespace Domain.Interfaces;

public interface ICheckInRepository
{
    ResponseRegisteredJson DoCheckIn(Guid attendeeId);
}