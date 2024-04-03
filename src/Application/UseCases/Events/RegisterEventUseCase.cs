using Application.Validators;
using AutoMapper;
using Communication.Requests;
using Communication.Responses;
using Domain.Entities;
using Exceptions;
using FluentValidation;
using Infrastructure.Context;

namespace Application.UseCases.Events;

public class RegisterEventUseCase
{
    private readonly PassInDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly EventValidator _validator;

    public RegisterEventUseCase(PassInDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _validator = new EventValidator(_dbContext);
    }

    public async Task<ResponseRegisteredJson> Execute(RequestEventJson request)
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
}