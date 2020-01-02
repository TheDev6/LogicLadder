namespace LogicLadder
{
    using System;
    using System.Collections.Generic;

    public class LadderResult<TContext>
    {
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public TimeSpan? Duration
        {
            get
            {
                if (this.Start.HasValue
                    && this.End.HasValue)
                {
                    return End.Value - Start.Value;
                }
                else
                {
                    return null;
                }
            }
        }
        public int TotalSteps { get; set; }
        public List<StepResult> CompletedSteps { get; set; } = new List<StepResult>();
        public LogicLadderExitStatus OutputStatus { get; set; } = LogicLadderExitStatus.None;
        public TContext Context { get; set; }
    }
}
