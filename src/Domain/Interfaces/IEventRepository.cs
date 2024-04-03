using Communication.Requests;
using Communication.Responses;

namespace Domain.Interfaces;

public interface IEventRepository
{
    ResponseRegisteredJson CreateNewEvent(RequestEventJson request);

    ResponseEventJson GetEventById(Guid id);
}