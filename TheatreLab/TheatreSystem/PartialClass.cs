namespace TheatreSystem
{
    using System;
    using System.Linq;
    using System.Text;

    internal partial class Theatre
    {
        // TODO: Fix partial classes!!!
        public static string ExecutePrintAllPerformancesCommand()
        {
            var performances = universal.ListAllPerformances().ToList();
            var result = String.Empty;
            if (performances.Any())
            {
                for (var i = 0; i < performances.Count; i++)
                {
                    var sb = new StringBuilder();
                    sb.Append(result);
                    if (i > 0)
                    {
                        sb.Append(", ");
                    }

                    var result1 = performances[i].StartTimes.ToString("dd.MM.yyyy HH:mm");
                    sb.AppendFormat("({0}, {1}, {2})", performances[i].PerformanceTitle, performances[i].TheatreName, result1);
                    result = sb + "";
                }
                return result;
            }

            return "No performances";
        }
    }
}