namespace LogicLadder_UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using FluentAssertions;
    using LogicLadder;
    using TestHelpers;
    using Xunit;

    public class Tests
    {
        [Fact]
        public async void Run_HappyPath()
        {
            var sut = new Ladder<TestLadderContext>(
                new List<IStep<TestLadderContext>>
                {
                    new DateStep(),
                    new NumberStep(),
                    new StringStep()
                });

            var result = await sut.Run(new TestLadderContext(), new CancellationToken());

            result.OutputStatus.Should().Be(LogicLadderExitStatus.Success);
            result.Start.Should().NotBeNull();
            result.End.Should().NotBeNull();
            result.Duration.Should().NotBeNull();
            result.TotalSteps.Should().Be(3);
            result.CompletedSteps.Should().NotBeNullOrEmpty();
            result.CompletedSteps.Count.Should().Be(3);

            result.CompletedSteps[0].Exception.Should().BeNull();
            result.CompletedSteps[0].Start.Should().NotBeNull();
            result.CompletedSteps[0].End.Should().NotBeNull();
            result.CompletedSteps[0].Duration.Should().NotBeNull();
            result.CompletedSteps[0].Duration.Should().BeGreaterThan(new TimeSpan(ticks: 1));
            result.CompletedSteps[0].StepName.Should().Be("DateStep");

            result.CompletedSteps[1].Exception.Should().BeNull();
            result.CompletedSteps[1].Start.Should().NotBeNull();
            result.CompletedSteps[1].End.Should().NotBeNull();
            result.CompletedSteps[1].Duration.Should().NotBeNull();
            result.CompletedSteps[1].StepName.Should().Be("NumberStep");

            result.CompletedSteps[2].Exception.Should().BeNull();
            result.CompletedSteps[2].Start.Should().NotBeNull();
            result.CompletedSteps[2].End.Should().NotBeNull();
            result.CompletedSteps[2].Duration.Should().NotBeNull();
            result.CompletedSteps[2].StepName.Should().Be("StringStep");

            result.Context.Should().NotBeNull();
            result.Context.Number.Should().NotBeNull();
            result.Context.Date.Should().NotBeNull();
            result.Context.String.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async void Run_Exception_ContinueOnFail()
        {
            var errorMessage = "Ka boom";
            var sut = new Ladder<TestLadderContext>(
                new List<IStep<TestLadderContext>>
                {
                    new DateStep(),
                    new NumberStep(),
                    new ExceptionStep(errorMessage,true),
                    new StringStep()
                });

            var result = await sut.Run(new TestLadderContext(), new CancellationToken());

            result.OutputStatus.Should().Be(LogicLadderExitStatus.SuccessWithErrors);
            result.Start.Should().NotBeNull();
            result.End.Should().NotBeNull();
            result.Duration.Should().NotBeNull();
            result.TotalSteps.Should().Be(4);
            result.CompletedSteps.Should().NotBeNullOrEmpty();
            result.CompletedSteps.Count.Should().Be(4);

            result.CompletedSteps[0].Exception.Should().BeNull();
            result.CompletedSteps[0].Start.Should().NotBeNull();
            result.CompletedSteps[0].End.Should().NotBeNull();
            result.CompletedSteps[0].Duration.Should().NotBeNull();
            result.CompletedSteps[0].StepName.Should().Be("DateStep");

            result.CompletedSteps[1].Exception.Should().BeNull();
            result.CompletedSteps[1].Start.Should().NotBeNull();
            result.CompletedSteps[1].End.Should().NotBeNull();
            result.CompletedSteps[1].Duration.Should().NotBeNull();
            result.CompletedSteps[1].StepName.Should().Be("NumberStep");

            result.CompletedSteps[2].Exception.Should().NotBeNull();
            result.CompletedSteps[2].Exception.Message.Should().Be(errorMessage);
            result.CompletedSteps[2].Start.Should().NotBeNull();
            result.CompletedSteps[2].End.Should().NotBeNull();
            result.CompletedSteps[2].Duration.Should().NotBeNull();
            result.CompletedSteps[2].StepName.Should().Be("ExceptionStep");

            result.CompletedSteps[3].Exception.Should().BeNull();
            result.CompletedSteps[3].Start.Should().NotBeNull();
            result.CompletedSteps[3].End.Should().NotBeNull();
            result.CompletedSteps[3].Duration.Should().NotBeNull();
            result.CompletedSteps[3].StepName.Should().Be("StringStep");

            result.Context.Should().NotBeNull();
            result.Context.Number.Should().NotBeNull();
            result.Context.Date.Should().NotBeNull();
            result.Context.String.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async void Run_Exception_StopOnFail()
        {
            var errorMessage = "Ka boom";
            var sut = new Ladder<TestLadderContext>(
                new List<IStep<TestLadderContext>>
                {
                    new DateStep(),
                    new ExceptionStep(errorMessage,false),
                    new NumberStep(),
                    new StringStep()
                });

            var result = await sut.Run(new TestLadderContext(), new CancellationToken());

            result.OutputStatus.Should().Be(LogicLadderExitStatus.Fail);
            result.Start.Should().NotBeNull();
            result.End.Should().NotBeNull();
            result.Duration.Should().NotBeNull();
            result.TotalSteps.Should().Be(4);
            result.CompletedSteps.Should().NotBeNullOrEmpty();
            result.CompletedSteps.Count.Should().Be(2);

            result.CompletedSteps[0].Exception.Should().BeNull();
            result.CompletedSteps[0].Start.Should().NotBeNull();
            result.CompletedSteps[0].End.Should().NotBeNull();
            result.CompletedSteps[0].StepName.Should().Be("DateStep");

            result.CompletedSteps[1].Exception.Should().NotBeNull();
            result.CompletedSteps[1].Exception.Message.Should().Be(errorMessage);
            result.CompletedSteps[1].Start.Should().NotBeNull();
            result.CompletedSteps[1].End.Should().NotBeNull();
            result.CompletedSteps[1].StepName.Should().Be("ExceptionStep");

            result.Context.Should().NotBeNull();
            result.Context.Date.Should().NotBeNull();
            result.Context.Number.Should().BeNull();//didn't make it that far
            result.Context.String.Should().BeNull();//didn't make it that far.
        }

        [Fact]
        public void Constructor_ThrowsErrorOnNullSteps()
        {
            var act = new Action(() => new Ladder<TestLadderContext>(steps: null));
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
