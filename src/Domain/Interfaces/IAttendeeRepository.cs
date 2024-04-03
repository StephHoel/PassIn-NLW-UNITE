using Communication.Requests;
using Communication.Responses;

namespace Domain.Interfaces;

public interface IAttendeeRepository
{
    ResponseRegisteredJson CreateNewAttendee(RequestRegisterEventJson request);
}