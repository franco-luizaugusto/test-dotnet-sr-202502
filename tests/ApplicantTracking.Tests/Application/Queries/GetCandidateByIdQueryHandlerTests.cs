using ApplicantTracking.Application.Abstractions;
using ApplicantTracking.Application.DTOs;
using ApplicantTracking.Application.Queries.GetById;
using ApplicantTracking.Domain.Entities;
using AutoMapper;
using FluentAssertions;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ApplicantTracking.Tests.Application.Queries;

public sealed class GetCandidateByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnNull_WhenCandidateNotFound()
    {
        var candidates = new Mock<ICandidateRepository>(MockBehavior.Strict);
        candidates
            .Setup(x => x.GetByIdAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Candidate?)null);

        var uow = new Mock<IUnitOfWork>(MockBehavior.Strict);
        uow.SetupGet(x => x.Candidates).Returns(candidates.Object);

        var mapper = new Mock<IMapper>(MockBehavior.Strict);

        var handler = new GetCandidateByIdQueryHandler(uow.Object, mapper.Object);

        var result = await handler.Handle(new GetCandidateByIdQuery(10), CancellationToken.None);

        result.Should().BeNull();
        candidates.VerifyAll();
        mapper.Verify(m => m.Map<CandidateDto>(It.IsAny<object>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnDto_WhenCandidateFound()
    {
        var candidate = new Candidate("John", "Doe", new DateTime(1990, 1, 1), "john@doe.com");

        var candidates = new Mock<ICandidateRepository>(MockBehavior.Strict);
        candidates
            .Setup(x => x.GetByIdAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(candidate);

        var uow = new Mock<IUnitOfWork>(MockBehavior.Strict);
        uow.SetupGet(x => x.Candidates).Returns(candidates.Object);

        var mapper = new Mock<IMapper>(MockBehavior.Strict);
        mapper
            .Setup(m => m.Map<CandidateDto>(candidate))
            .Returns(new CandidateDto { IdCandidate = 10 });

        var handler = new GetCandidateByIdQueryHandler(uow.Object, mapper.Object);

        var result = await handler.Handle(new GetCandidateByIdQuery(10), CancellationToken.None);

        result.Should().NotBeNull();
        result!.IdCandidate.Should().Be(10);

        candidates.VerifyAll();
        mapper.VerifyAll();
    }
}

