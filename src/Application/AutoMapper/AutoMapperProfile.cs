using AutoMapper;
using Communication.Requests;
using Communication.Responses;
using Domain.Entities;

namespace Application.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<RequestEventJson, Event>()
                .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Title.ToLower().Replace(" ", "-")))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title.Trim()))
                .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.Details.Trim()));

        CreateMap<Event, ResponseEventJson>()
            .ForMember(dest => dest.Attendees_Amount, opt => opt.MapFrom(_ => -1));

        CreateMap<Event, ResponseRegisteredEventJson>();
    }
}