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

public class EventRepository : IEventRepository
{
    private readonly PassInDbContext _dbContext;
    private readonly EventValidator _validator;

    private readonly IMapper _mapper;

    public EventRepository(PassInDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _validator = new EventValidator(_dbContext);

        _mapper = mapper;
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
        var entity = _dbContext.Events.Include(ev => ev.Attendees).FirstOrDefault(ev => ev.Id == id);

        if (entity is null)
            throw new NotFoundException("An event with this id does not exist.");

        return _mapper.Map<ResponseEventJson>(entity);
    }
}