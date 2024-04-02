using Communication.Responses;
using Exceptions;
using Infrastructure;

namespace Application.UseCases.Events;

public class GetEventByIdUseCase
{
    public ResponseEventJson Execute(Guid id)
    {
        var dbContext = new PassInDbContext();

        var entity = dbContext.Events.FirstOrDefault(e => e.Id == id);

        if (entity is null)
            throw new PassInException("An event with this id dont exist.");

        return new ResponseEventJson
        {
            Id = entity.Id,
            Title = entity.Title,
            Details = entity.Details,
            MaximumAttendees = entity.Maximum_Attendees,
            AttendeesAmount = -1,
        };
    }
}