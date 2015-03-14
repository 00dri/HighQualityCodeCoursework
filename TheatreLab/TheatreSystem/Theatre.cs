namespace TheatreSystem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;

    internal static partial class Theatre
    {
        public static IPerformanceDatabase universal = new PerformanceDatabase();

        internal static string ExecuteAddTheatreCommand(string[] parameters)
        {
            var theatreName = parameters[0];
            universal.AddTheatre(theatreName);
            return "Theatre added";
        }

        public static string ExecutePrintAllTheatresCommand()
        {
            var theatresCount = universal.ListTheatres().Count();
            if (theatresCount > 0)
            {
                var resultTheatres = new LinkedList<string>();
                for (var i = 0; i < theatresCount; i++)
                {
                    universal.ListTheatres().Skip(i).ToList().ForEach(t => resultTheatres.AddLast(t));
                    foreach (var t in universal.ListTheatres().Skip(i + 1))
                    {
                        resultTheatres.Remove(t);
                    }
                }
                return String.Join(", ", resultTheatres);
            }
            return "No theatres";
        }
    }
}