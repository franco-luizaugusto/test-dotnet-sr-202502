using ApplicantTracking.Application.Abstractions;
using ApplicantTracking.Application.DTOs;
using ApplicantTracking.Application.Exceptions;
using ApplicantTracking.Domain.Entities;
using ApplicantTracking.Domain.Events;
using AutoMapper;
using MediatR;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicantTracking.Application.Commands.Create;

public sealed class CreateCandidateCommandHandler : IRequestHandler<CreateCandidateCommand, CandidateDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CreateCandidateCommandHandler(IUnitOfWork uow, IMediator mediator, IMapper mapper)
    {
        _uow = uow;
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<CandidateDto> Handle(CreateCandidateCommand request, CancellationToken cancellationToken)
    {
        var emailExists = await _uow.Candidates.EmailExistsAsync(request.Candidate.Email, cancellationToken);
        if (emailExists)
        {
            throw new BusinessRuleException("Email already exists.");
        }

        var candidate = new Candidate(
            request.Candidate.Name,
            request.Candidate.Surename,
            request.Candidate.Birthdate,
            request.Candidate.Email);

        await _uow.Candidates.AddAsync(candidate, cancellationToken);

        await using var tx = await _uow.BeginTransactionAsync(cancellationToken);

        await _uow.SaveChangesAsync(cancellationToken);

        var newData = JsonSerializer.Serialize(candidate);
        await _mediator.Publish(
            new CandidateCreatedEvent(
                IdAggregateRoot: candidate.IdCandidate,
                OldData: null,
                NewData: newData,
                OccurredAtUtc: System.DateTime.UtcNow),
            cancellationToken);

        await _uow.SaveChangesAsync(cancellationToken);
        await tx.CommitAsync(cancellationToken);

        return _mapper.Map<CandidateDto>(candidate);
    }
}

