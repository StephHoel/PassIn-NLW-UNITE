using Communication.Requests;
using Communication.Responses;

namespace Domain.Interfaces;

public interface IAttendeeRepository
{
    ResponseRegisteredJson CreateNewAttendee(Guid eventId, RequestRegisterEventJson request);

    ResponseAllAttendeesJson GetAllByEventId(Guid eventId);
}