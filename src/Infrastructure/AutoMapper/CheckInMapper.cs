using AutoMapper;
using Communication.Responses;
using Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.AutoMapper;

[ExcludeFromCodeCoverage]
public class CheckInMapper : Profile
{
    public CheckInMapper()
    {
        CreateMap<Guid, CheckIn>()
            .ForMember(dest => dest.Attendee_Id, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.Created_at, opt => opt.MapFrom(_ => DateTime.UtcNow));

        CreateMap<CheckIn, ResponseRegisteredJson>();
    }
}