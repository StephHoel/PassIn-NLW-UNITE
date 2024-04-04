using Domain.Shared;
using AutoMapper;
using Communication.Requests;
using Communication.Responses;
using Domain.Entities;
using Domain.Interfaces;
using Exceptions;
using Infrastructure.Context;
using Infrastructure.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Infrastructure.Repositories;

public class EventRepository : IEventRepository
{
    private readonly PassInDbContext _dbContext;
    private readonly EventValidator _validator;

    private readonly IMapper _mapper;
    private readonly IStringLocalizer<ErrorMessages> _stringLocalizer;

    public EventRepository(PassInDbContext dbContext, IMapper mapper, IStringLocalizer<ErrorMessages> stringLocalizer)
    {
        _dbContext = dbContext;
        _validator = new EventValidator(_dbContext, stringLocalizer);

        _mapper = mapper;
        _stringLocalizer = stringLocalizer;
    }

    public ResponseRegisteredJson CreateNewEvent(RequestEventJson request)
    {
        var entity = _mapper.Map<Event>(request);

        var result = _validator.Validate(entity);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                throw new ErrorOnValidationException(error.ErrorMessage);
        }

        _dbContext.Events.Add(entity);
        _dbContext.SaveChanges();

        return _mapper.Map<ResponseRegisteredJson>(entity);
    }

    public ResponseEventJson GetEventById(Guid id)
    {
        var entity = _dbContext.Events
            .Include(ev => ev.Attendees)
            .FirstOrDefault(ev => ev.Id == id);

        if (entity is null)
            throw new NotFoundException(_stringLocalizer["EventNotExist"]);

        return _mapper.Map<ResponseEventJson>(entity);
    }
}