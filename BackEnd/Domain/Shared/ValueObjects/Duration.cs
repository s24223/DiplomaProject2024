namespace Domain.Shared.ValueObjects
{
    public record Duration
    {
        //Values
        public int Years { get; private set; }
        public int Months { get; private set; }
        public int Days { get; private set; }


        //Constructor
        public Duration(DateOnly start, DateOnly end)
        {
            if (end < start)
            {
                var syntetic = start;
                start = end;
                end = syntetic;
            }
            var time = new DateOnly().AddDays(end.DayNumber - start.DayNumber);
            Years = time.Year - 1;
            Months = time.Month - 1;
            Days = time.Day - 1;
        }
    }
}
