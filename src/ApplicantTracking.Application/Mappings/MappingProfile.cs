using ApplicantTracking.Application.DTOs;
using ApplicantTracking.Domain.Entities;
using AutoMapper;

namespace ApplicantTracking.Application.Mappings;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Candidate, CandidateDto>();
    }
}

