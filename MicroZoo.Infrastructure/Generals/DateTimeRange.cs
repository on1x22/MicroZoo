
namespace MicroZoo.Infrastructure.Generals
{
    public record DateTimeRange
    {
        public DateTime StartDateTime { get; set; }
        public DateTime FinishDateTime { get; set; }

        public DateTimeRange(DateTime startDateTime, DateTime finishDateTime)
        {
            StartDateTime = startDateTime;
            FinishDateTime = finishDateTime;
        }
    }
}
