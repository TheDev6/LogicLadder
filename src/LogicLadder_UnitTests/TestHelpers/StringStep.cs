namespace LogicLadder_UnitTests.TestHelpers
{
    using System.Threading.Tasks;
    using LogicLadder;

    public class StringStep : IStep<TestLadderContext>
    {
        public bool ContinueOnError => false;
        public Task<TestLadderContext> RunStep(TestLadderContext context)
        {
            context.String = "String step";
            return Task.FromResult(context);
        }
    }
}
