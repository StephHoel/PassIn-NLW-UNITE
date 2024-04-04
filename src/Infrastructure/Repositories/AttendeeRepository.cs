using AutoMapper;
using Communication.Requests;
using Communication.Responses;
using Domain.Entities;
using Domain.Interfaces;
using Exceptions;
using Infrastructure.Context;
using Infrastructure.Validators;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AttendeeRepository : IAttendeeRepository
{
    private readonly PassInDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly AttendeeValidator _validator;

    public AttendeeRepository(PassInDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _validator = new AttendeeValidator();
    }

    public ResponseRegisteredJson CreateNewAttendee(Guid eventId, RequestRegisterEventJson request)
    {
        var entity = _mapper.Map<Attendee>(request);
        entity.Event_Id = eventId;

        Validate(entity);

        _dbContext.Attendees.Add(entity);
        _dbContext.SaveChanges();

        return _mapper.Map<ResponseRegisteredJson>(entity);
    }

    public ResponseAllAttendeesJson GetAllByEventId(Guid eventId)
    {
        var entity = _dbContext.Events
            .Include(ev => ev.Attendees)
            .ThenInclude(at => at.CheckIn)
            .FirstOrDefault(ev => ev.Id == eventId);

        if (entity is null)
            throw new NotFoundException("An event with this id does not exist.");

        return _mapper.Map<List<Attendee>, ResponseAllAttendeesJson>(entity.Attendees);
    }

    private void Validate(Attendee attendee)
    {
        var eventEntity = _dbContext.Events.FirstOrDefault(e => e.Id == attendee.Event_Id);

        if (eventEntity is null)
            throw new NotFoundException("An event with this id does not exist.");

        var result = _validator.Validate(attendee);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                throw new ErrorOnValidationException(error.ErrorMessage);
        }

        var attendeeAlreadyRegistered = _dbContext.Attendees
            .Any(a => a.Email.Equals(attendee.Email) && a.Event_Id == attendee.Event_Id);

        if (attendeeAlreadyRegistered is true)
            throw new ConflictException("You cannot register twice on the same event.");

        var attendeeForEvent = _dbContext.Attendees.Count(x => x.Event_Id == attendee.Event_Id);

        if (attendeeForEvent >= eventEntity.Maximum_Attendees)
            throw new ConflictException("There is no room for this event.");
    }
}