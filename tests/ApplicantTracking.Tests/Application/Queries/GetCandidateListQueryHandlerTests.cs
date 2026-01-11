using ApplicantTracking.Application.Abstractions;
using ApplicantTracking.Application.DTOs;
using ApplicantTracking.Application.Queries.List;
using ApplicantTracking.Domain.Entities;
using AutoMapper;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ApplicantTracking.Tests.Application.Queries;

public sealed class GetCandidateListQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnMappedDtos()
    {
        var entities = new List<Candidate>
        {
            new Candidate("A", "B", new DateTime(1990, 1, 1), "a@b.com"),
            new Candidate("C", "D", new DateTime(1991, 1, 1), "c@d.com")
        };

        var candidates = new Mock<ICandidateRepository>(MockBehavior.Strict);
        candidates
            .Setup(x => x.ListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(entities);

        var uow = new Mock<IUnitOfWork>(MockBehavior.Strict);
        uow.SetupGet(x => x.Candidates).Returns(candidates.Object);

        var mapped = new List<CandidateDto>
        {
            new CandidateDto { IdCandidate = 1 },
            new CandidateDto { IdCandidate = 2 }
        };

        var mapper = new Mock<IMapper>(MockBehavior.Strict);
        mapper
            .Setup(m => m.Map<List<CandidateDto>>(entities))
            .Returns(mapped);

        var handler = new GetCandidateListQueryHandler(uow.Object, mapper.Object);

        var result = await handler.Handle(new GetCandidateListQuery(), CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result[0].IdCandidate.Should().Be(1);
        result[1].IdCandidate.Should().Be(2);

        candidates.VerifyAll();
        mapper.VerifyAll();
    }
}

