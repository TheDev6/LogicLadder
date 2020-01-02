namespace LogicLadder_UnitTests.TestHelpers
{
    using System;
    using System.Threading.Tasks;
    using LogicLadder;

    public class NumberStep : IStep<TestLadderContext>
    {
        private Random _rando = new Random();
        public bool ContinueOnError => false;
        public Task<TestLadderContext> RunStep(TestLadderContext context)
        {
            context.Number = _rando.Next(1, 100);
            return Task.FromResult(context);
        }
    }
}
