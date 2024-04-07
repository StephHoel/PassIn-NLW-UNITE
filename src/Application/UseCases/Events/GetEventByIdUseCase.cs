using Communication.Responses;
using Domain.Interfaces;

namespace Application.UseCases.Events;

public class GetEventByIdUseCase
{
    private readonly IEventRepository _repository;

    public GetEventByIdUseCase(IEventRepository repository)
    {
        _repository = repository;
    }

    public ResponseEventJson Execute(Guid id)
    {
        var response = _repository.GetEventById(id);

        return response;
    }
}