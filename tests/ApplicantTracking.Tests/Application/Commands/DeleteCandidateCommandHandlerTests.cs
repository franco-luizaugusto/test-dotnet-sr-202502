using ApplicantTracking.Application.Abstractions;
using ApplicantTracking.Application.Commands.Delete;
using ApplicantTracking.Domain.Entities;
using ApplicantTracking.Domain.Events;
using ApplicantTracking.Tests.TestUtils;
using FluentAssertions;
using MediatR;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ApplicantTracking.Tests.Application.Commands;

public sealed class DeleteCandidateCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenCandidateNotFound()
    {
        var candidates = new Mock<ICandidateRepository>(MockBehavior.Strict);
        candidates
            .Setup(x => x.GetByIdAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Candidate?)null);

        var uow = new Mock<IUnitOfWork>(MockBehavior.Strict);
        uow.SetupGet(x => x.Candidates).Returns(candidates.Object);

        var mediator = new Mock<IMediator>(MockBehavior.Strict);

        var handler = new DeleteCandidateCommandHandler(uow.Object, mediator.Object);

        var result = await handler.Handle(new DeleteCandidateCommand(10), CancellationToken.None);

        result.Should().BeFalse();
        candidates.VerifyAll();
        uow.VerifyAll();
    }

    [Fact]
    public async Task Handle_ShouldRemoveCandidate_PublishDeletedEvent_AndCommit_WhenCandidateFound()
    {
        var candidate = new Candidate("John", "Doe", new DateTime(1990, 1, 1), "john@doe.com");
        ReflectionHelper.SetPrivateProperty(candidate, "IdCandidate", 77);

        var candidates = new Mock<ICandidateRepository>(MockBehavior.Strict);
        candidates
            .Setup(x => x.GetByIdAsync(77, It.IsAny<CancellationToken>()))
            .ReturnsAsync(candidate);
        candidates
            .Setup(x => x.Remove(candidate));

        var tx = new Mock<ITransaction>(MockBehavior.Strict);
        tx.Setup(x => x.CommitAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        tx.Setup(x => x.DisposeAsync()).Returns(ValueTask.CompletedTask);

        var uow = new Mock<IUnitOfWork>(MockBehavior.Strict);
        uow.SetupGet(x => x.Candidates).Returns(candidates.Object);
        uow.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>())).ReturnsAsync(tx.Object);
        uow.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var mediator = new Mock<IMediator>(MockBehavior.Strict);
        mediator
            .Setup(x => x.Publish(It.IsAny<CandidateDeletedEvent>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new DeleteCandidateCommandHandler(uow.Object, mediator.Object);

        var result = await handler.Handle(new DeleteCandidateCommand(77), CancellationToken.None);

        result.Should().BeTrue();
        candidates.Verify(x => x.Remove(candidate), Times.Once);
        uow.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
        mediator.Verify(x => x.Publish(It.IsAny<CandidateDeletedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        tx.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}

