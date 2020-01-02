namespace LogicLadder_UnitTests.TestHelpers
{
    using System;
    using System.Threading.Tasks;
    using LogicLadder;

    public class DateStep : IStep<TestLadderContext>
    {
        public bool ContinueOnError => false;

        public Task<TestLadderContext> RunStep(TestLadderContext context)
        {
            context.Date = DateTime.Now;
            return Task.FromResult(context);
        }
    }
}
