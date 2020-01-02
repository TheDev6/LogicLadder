namespace LogicLadder_UnitTests.TestHelpers
{
    using System.Threading.Tasks;
    using LogicLadder;

    public class ExceptionStep : IStep<TestLadderContext>
    {
        private readonly string _errorMessage;
        private readonly bool _continueOnError;
        public ExceptionStep(string errorMessage, bool continueOnError)
        {
            _errorMessage = errorMessage;
            _continueOnError = continueOnError;
        }
        public bool ContinueOnError => _continueOnError;
        public Task<TestLadderContext> RunStep(TestLadderContext context)
        {
            throw new System.Exception(_errorMessage);
        }
    }
}
