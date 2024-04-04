using AutoMapper;
using Communication.Requests;
using Communication.Responses;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Shared;
using Exceptions;
using Infrastructure.Context;
using Infrastructure.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Infrastructure.Repositories;

public class AttendeeRepository : IAttendeeRepository
{
    private readonly PassInDbContext _dbContext;
    private readonly AttendeeValidator _validator;

    private readonly IMapper _mapper;
    private readonly IStringLocalizer<ErrorMessages> _stringLocalizer;

    public AttendeeRepository(PassInDbContext dbContext,
                              IMapper mapper,
                              IStringLocalizer<ErrorMessages> stringLocalizer)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _validator = new AttendeeValidator(stringLocalizer);
        _stringLocalizer = stringLocalizer;
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
            throw new NotFoundException(_stringLocalizer["EventNotExist"]);

        return _mapper.Map<List<Attendee>, ResponseAllAttendeesJson>(entity.Attendees);
    }

    private void Validate(Attendee attendee)
    {
        var eventEntity = _dbContext.Events.FirstOrDefault(e => e.Id == attendee.Event_Id);

        if (eventEntity is null)
            throw new NotFoundException(_stringLocalizer["EventNotExist"]);

        var result = _validator.Validate(attendee);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                throw new ErrorOnValidationException(error.ErrorMessage);
        }

        var attendeeAlreadyRegistered = _dbContext.Attendees
            .Any(a => a.Email.Equals(attendee.Email) && a.Event_Id == attendee.Event_Id);

        if (attendeeAlreadyRegistered is true)
            throw new ConflictException(_stringLocalizer["RegisterTwiceSameEvent"]);

        var attendeeForEvent = _dbContext.Attendees.Count(x => x.Event_Id == attendee.Event_Id);

        if (attendeeForEvent >= eventEntity.Maximum_Attendees)
            throw new ConflictException(_stringLocalizer["NoRoomEvent"]);
    }
}