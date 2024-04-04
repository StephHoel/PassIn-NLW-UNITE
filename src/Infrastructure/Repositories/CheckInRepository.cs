using AutoMapper;
using Communication.Responses;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Shared;
using Exceptions;
using Infrastructure.Context;
using Infrastructure.Validators;
using Microsoft.Extensions.Localization;

namespace Infrastructure.Repositories;

public class CheckInRepository : ICheckInRepository
{
    private readonly PassInDbContext _dbContext;

    private readonly IMapper _mapper;
    private readonly IStringLocalizer<ErrorMessages> _stringLocalizer;

    public CheckInRepository(PassInDbContext dbContext,
                             IMapper mapper,
                             IStringLocalizer<ErrorMessages> stringLocalizer)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _stringLocalizer = stringLocalizer;
    }

    public ResponseRegisteredJson DoCheckIn(Guid attendeeId)
    {
        Validate(attendeeId);

        var entity = _mapper.Map<CheckIn>(attendeeId);

        _dbContext.CheckIns.Add(entity);
        _dbContext.SaveChanges();

        return _mapper.Map<ResponseRegisteredJson>(entity);
    }

    private void Validate(Guid attendeeId)
    {
        var existAttendee = _dbContext.Attendees.Any(at => at.Id == attendeeId);

        if (existAttendee is false)
            throw new NotFoundException(_stringLocalizer["AttendeeNotFound"]);

        var existCheckIn = _dbContext.CheckIns.Any(ch => ch.Attendee_Id == attendeeId);

        if (existCheckIn is true)
            throw new ConflictException(_stringLocalizer["AttendeeTwiceChecking"]);
    }
}