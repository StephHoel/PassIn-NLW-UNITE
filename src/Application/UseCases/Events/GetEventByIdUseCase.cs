using AutoMapper;
using Communication.Responses;
using Exceptions;
using Infrastructure.Context;

namespace Application.UseCases.Events;

public class GetEventByIdUseCase
{
    private readonly PassInDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetEventByIdUseCase(PassInDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public ResponseEventJson Execute(Guid id)
    {
        var entity = _dbContext.Events.FirstOrDefault(e => e.Id == id);

        if (entity is null)
            throw new PassInException("An event with this id dont exist.");

        return _mapper.Map<ResponseEventJson>(entity);
    }
}