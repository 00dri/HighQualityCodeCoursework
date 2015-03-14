namespace TheatreSystem
{
    using System;

    public class Performance : IComparable<Performance>
    {
        public Performance(string theatreName, string performanceTitle, DateTime startTimes, TimeSpan duration, decimal price)
        {
            this.TheatreName = theatreName;
            this.PerformanceTitle = performanceTitle;
            this.StartTimes = startTimes;
            this.Duration = duration;
            this.Price = price;
        }

        public string TheatreName { get; protected internal set; }

        public string PerformanceTitle { get; private set; }

        public DateTime StartTimes { get; set; }

        public TimeSpan Duration { get; private set; }

        protected internal decimal Price { get; protected set; }

        int IComparable<Performance>.CompareTo(Performance otherPerformance)
        {
            var tmp = this.StartTimes.CompareTo(otherPerformance.StartTimes);
            return tmp;
        }

        public override string ToString()
        {
            var result = string.Format(
                "Performance(Theatre: {0}; Performance: {1}; StartTime: {2}, Duration: {3}, Price: {4})",
                this.TheatreName,
                this.PerformanceTitle,
                this.StartTimes.ToString("dd.MM.yyyy HH:mm"), this.Duration.ToString("hh':'mm"), this.Price.ToString("f2"));
            return result;
        }
    }
}