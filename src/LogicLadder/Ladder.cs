namespace LogicLadder
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class Ladder<TContext> where TContext : class
    {
        private readonly List<IStep<TContext>> _steps;
        private readonly Action<Exception> _onError;
        private readonly bool _useUtc;

        public Ladder(
            List<IStep<TContext>> steps,
            Action<Exception> onError = null,
            bool useUtc = false)
        {
            _steps = steps ?? throw new ArgumentNullException(nameof(steps));
            _useUtc = useUtc;
            _onError = onError;
        }

        public async Task<LadderResult<TContext>> Run(TContext input, CancellationToken cancellationToken)
        {
            var result = new LadderResult<TContext>()
            {
                OutputStatus = LogicLadderExitStatus.Success,
                Start = this.TimeStamp(),
                TotalSteps = _steps.Count,
                CompletedSteps = new List<StepResult>()
            };
            var runningT = input;
            foreach (var step in _steps)
            {
                if (cancellationToken.IsCancellationRequested == false)
                {
                    var stepResult = new StepResult() { Start = this.TimeStamp(), StepName = step.GetType().Name };
                    try
                    {
                        runningT = await step.RunStep(runningT);
                    }
                    catch (Exception ex)
                    {
                        stepResult.Exception = ex;
                        if (step.ContinueOnError)
                        {
                            result.OutputStatus = LogicLadderExitStatus.SuccessWithErrors;
                        }
                        else
                        {
                            result.OutputStatus = LogicLadderExitStatus.Fail;
                            stepResult.End = DateTime.Now;
                            result.CompletedSteps.Add(stepResult);
                            break;
                        }
                        _onError?.Invoke(ex);
                    }
                    stepResult.End = this.TimeStamp();
                    result.CompletedSteps.Add(stepResult);
                }
                else
                {
                    result.OutputStatus = LogicLadderExitStatus.Cancelled;
                    break;
                }
            }
            result.End = this.TimeStamp();
            result.Context = runningT;
            return result;
        }

        private DateTime TimeStamp()
        {
            return _useUtc ? DateTime.UtcNow : DateTime.Now;
        }
    }
}
