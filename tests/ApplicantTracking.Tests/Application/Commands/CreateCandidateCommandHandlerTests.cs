using ApplicantTracking.Application.Abstractions;
using ApplicantTracking.Application.Commands.Create;
using ApplicantTracking.Application.DTOs;
using ApplicantTracking.Application.Exceptions;
using ApplicantTracking.Domain.Events;
using ApplicantTracking.Tests.TestUtils;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ApplicantTracking.Tests.Application.Commands;

public sealed class CreateCandidateCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldThrowBusinessRuleException_WhenEmailAlreadyExists()
    {
        var candidates = new Mock<ICandidateRepository>(MockBehavior.Strict);
        candidates
            .Setup(x => x.EmailExistsAsync("test@example.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var uow = new Mock<IUnitOfWork>(MockBehavior.Strict);
        uow.SetupGet(x => x.Candidates).Returns(candidates.Object);

        var mediator = new Mock<IMediator>(MockBehavior.Strict);
        var mapper = new Mock<IMapper>(MockBehavior.Strict);

        var handler = new CreateCandidateCommandHandler(uow.Object, mediator.Object, mapper.Object);

        var cmd = new CreateCandidateCommand(new CreateCandidateDto
        {
            Name = "John",
            Surename = "Doe",
            Birthdate = new DateTime(1990, 1, 1),
            Email = "test@example.com"
        });

        Func<Task> act = async () => await handler.Handle(cmd, CancellationToken.None);

        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage("Email already exists.");

        candidates.VerifyAll();
        uow.VerifyAll();
    }

    [Fact]
    public async Task Handle_ShouldCreateCandidate_PublishEvent_AndCommitTransaction()
    {
        object? createdCandidate = null;
        var saveCalls = 0;

        var candidates = new Mock<ICandidateRepository>(MockBehavior.Strict);
        candidates
            .Setup(x => x.EmailExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        candidates
            .Setup(x => x.AddAsync(It.IsAny<ApplicantTracking.Domain.Entities.Candidate>(), It.IsAny<CancellationToken>()))
            .Callback<object, CancellationToken>((c, _) => createdCandidate = c)
            .Returns(Task.CompletedTask);

        var tx = new Mock<ITransaction>(MockBehavior.Strict);
        tx.Setup(x => x.CommitAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        tx.Setup(x => x.DisposeAsync()).Returns(ValueTask.CompletedTask);

        var uow = new Mock<IUnitOfWork>(MockBehavior.Strict);
        uow.SetupGet(x => x.Candidates).Returns(candidates.Object);
        uow.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>())).ReturnsAsync(tx.Object);
        uow.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Callback(() =>
            {
                saveCalls++;
                if (saveCalls == 1 && createdCandidate is not null)
                {
                    ReflectionHelper.SetPrivateProperty(createdCandidate, "IdCandidate", 123);
                }
            })
            .ReturnsAsync(1);

        var mediator = new Mock<IMediator>(MockBehavior.Strict);
        mediator
            .Setup(x => x.Publish(It.IsAny<CandidateCreatedEvent>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var mapper = new Mock<IMapper>(MockBehavior.Strict);
        mapper
            .Setup(x => x.Map<CandidateDto>(It.IsAny<object>()))
            .Returns<object>(src =>
            {
                var id = (int)src.GetType().GetProperty("IdCandidate")!.GetValue(src)!;
                return new CandidateDto { IdCandidate = id };
            });

        var handler = new CreateCandidateCommandHandler(uow.Object, mediator.Object, mapper.Object);

        var cmd = new CreateCandidateCommand(new CreateCandidateDto
        {
            Name = "John",
            Surename = "Doe",
            Birthdate = new DateTime(1990, 1, 1),
            Email = "john.doe@example.com"
        });

        var result = await handler.Handle(cmd, CancellationToken.None);

        result.IdCandidate.Should().Be(123);

        candidates.Verify(x => x.AddAsync(It.IsAny<ApplicantTracking.Domain.Entities.Candidate>(), It.IsAny<CancellationToken>()), Times.Once);
        uow.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
        mediator.Verify(x => x.Publish(It.IsAny<CandidateCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        tx.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}

