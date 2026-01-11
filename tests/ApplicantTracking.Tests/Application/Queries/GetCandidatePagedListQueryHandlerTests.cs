using ApplicantTracking.Application.Abstractions;
using ApplicantTracking.Application.DTOs;
using ApplicantTracking.Application.Queries.Paged;
using AutoMapper;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ApplicantTracking.Tests.Application.Queries;

public sealed class GetCandidatePagedListQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnPagedListWithCorrectMetadata()
    {
        var candidates = new Mock<ICandidateRepository>(MockBehavior.Strict);
        candidates
            .Setup(x => x.ListPagedAsync(2, 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync((new List<ApplicantTracking.Domain.Entities.Candidate>(), 12));

        var uow = new Mock<IUnitOfWork>(MockBehavior.Strict);
        uow.SetupGet(x => x.Candidates).Returns(candidates.Object);

        var mapper = new Mock<IMapper>(MockBehavior.Strict);
        mapper
            .Setup(x => x.Map<List<CandidateDto>>(It.IsAny<object>()))
            .Returns(new List<CandidateDto>());

        var handler = new GetCandidatePagedListQueryHandler(uow.Object, mapper.Object);

        var result = await handler.Handle(new GetCandidatePagedListQuery(2, 5), CancellationToken.None);

        result.TotalCount.Should().Be(12);
        result.PageSize.Should().Be(5);
        result.CurrentPage.Should().Be(2);
        result.TotalPages.Should().Be(3);
        result.HasPreviousPage.Should().BeTrue();
        result.HasNextPage.Should().BeTrue();
    }
}

