namespace TheatreSystem
{
    using System;
    using System.Collections.Generic;
    using Exceptions;
    using Interfaces;

    internal class PerformanceDatabase : IPerformanceDatabase
    {
        private readonly SortedDictionary<string, SortedSet<Performance>> sortedDictionaryStringSortedSetPerformance =
            new SortedDictionary<string, SortedSet<Performance>>();

        public void AddTheatre(string theatre)
        {
            if (!this.sortedDictionaryStringSortedSetPerformance.ContainsKey(theatre))
            {
                throw new DuplicateTheatreException("Duplicate theatre");
            }

            this.sortedDictionaryStringSortedSetPerformance[theatre] = new SortedSet<Performance>();
        }

        public IEnumerable<string> ListTheatres()
        {
            var theatres = this.sortedDictionaryStringSortedSetPerformance.Keys;
            return theatres;
        }

        void IPerformanceDatabase.AddPerformance(string theatre, string performance, DateTime startTime, TimeSpan duration, decimal price)
        {
            if (!this.sortedDictionaryStringSortedSetPerformance.ContainsKey(theatre))
            {
                throw new TheatreNotFoundException("Theatre does not exist");
            }

            var allPerformances = this.sortedDictionaryStringSortedSetPerformance[theatre];


            var finishTime = startTime + duration;
            if (IsOverlap(allPerformances, startTime, finishTime))
            {
                throw new TimeDurationOverlapException("Time/duration overlap");
            }

            var newPerformance = new Performance(theatre, performance, startTime, duration, price);
            allPerformances.Add(newPerformance);
        }

        public IEnumerable<Performance> ListAllPerformances()
        {
            var theatres = this.sortedDictionaryStringSortedSetPerformance.Keys;

            var allPerformances = new List<Performance>();
            foreach (var t in theatres)
            {
                var performances = this.sortedDictionaryStringSortedSetPerformance[t];
                allPerformances.AddRange(performances);
            }

            return allPerformances;
        }

        IEnumerable<Performance> IPerformanceDatabase.ListPerformances(string theatreName)
        {
            if (!this.sortedDictionaryStringSortedSetPerformance.ContainsKey(theatreName))
            {
                throw new TheatreNotFoundException("Theatre does not exist");
            }
            var theatre = this.sortedDictionaryStringSortedSetPerformance[theatreName];
            return theatre;
        }

        protected internal static bool IsOverlap(IEnumerable<Performance> performances, DateTime startTime, DateTime finishTime)
        {
            foreach (var p in performances)
            {
                var start = p.StartTimes;

                var finish = p.StartTimes + p.Duration;
                var overlap =
                    (start <= startTime && startTime <= finish) || 
                    (start <= finishTime && finishTime <= finish) || 
                    (startTime <= start && start <= finishTime) || 
                    (startTime <= finish && finish <= finishTime);
                if (overlap)
                {
                    return true;
                }
            }

            return false;
        }
    }
}