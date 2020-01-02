namespace LogicLadder
{
    using System;

    public class StepResult
    {
        public string StepName { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public TimeSpan? Duration
        {
            get
            {
                if (this.Start.HasValue
                    & this.End.HasValue)
                {
                    return End.Value - Start.Value;
                }
                else
                {
                    return null;
                }
            }
        }
        public Exception Exception { get; set; }
    }
}
