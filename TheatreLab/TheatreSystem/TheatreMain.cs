namespace TheatreSystem
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading;

    // Do not modify the interface members
    // Moving the interface to separate namespace is allowed
    // Adding XML documentation is allowed
    // TODO: document this interface definition


    internal partial class TheatreMain
    {
        protected static void Main()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("vi-VN");

            while (true)
            {
                var input = Console.ReadLine();
                if (input == null)
                {
                    return;
                }

                if (input != string.Empty)
                {
                    var splittedInput = input.Split('(');
                    var command = splittedInput[0];
                    string commandResult;
                    try
                    {
                        switch (command)
                        {
                            case "AddTheatre":
                                splittedInput = input.Split('(');
                                command = splittedInput[0];

                                var chiHuyParts1 = input.Split(new[] { '(', ',', ')' }, StringSplitOptions.RemoveEmptyEntries);
                                var chiHuyParams = chiHuyParts1.Skip(1).Select(p => p.Trim()).ToArray();
                                commandResult = Theatre.ExecuteAddTheatreCommand(chiHuyParams);
                                break;
                            case "PrintAllTheaters":
                                commandResult = Theatre.ExecutePrintAllTheatresCommand();
                                break;
                            case "AddPerformance":
                                splittedInput = input.Split('(');
                                command = splittedInput[0];
                                chiHuyParts1 = input.Split(new[] { '(', ',', ')' }, StringSplitOptions.RemoveEmptyEntries);
                                chiHuyParams = chiHuyParts1.Skip(1).Select(p => p.Trim()).ToArray();

                                var theatreName = chiHuyParams[0];
                                var performanceTitle = chiHuyParams[1];
                                var startDateTime = DateTime.ParseExact(chiHuyParams[2], "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
                                var duration = TimeSpan.Parse(chiHuyParams[3]);
                                var price = decimal.Parse(chiHuyParams[4], NumberStyles.Float);
                                universal.AddPerformance(theatreName, performanceTitle, startDateTime, duration, price);
                                commandResult = "Performance added";
                                break;
                            case "PrintAllPerformances":
                                commandResult = ExecutePrintAllPerformancesCommand();
                                break;
                            case "PrintPerformances":
                                splittedInput = input.Split('(');
                                command = splittedInput[0];
                                chiHuyParts1 = input.Split(new[] { '(', ',', ')' }, StringSplitOptions.RemoveEmptyEntries);
                                chiHuyParams = chiHuyParts1.Skip(1).Select(p => p.Trim()).ToArray();

                                var theatre = chiHuyParams[0];
                                var performances = universal.ListPerformances(theatre).Select(p =>
                                    {
                                        var result1 = p.StartTimes.ToString("dd.MM.yyyy HH:mm");
                                        return string.Format("({0}, {1})", p.PerformanceTitle, result1);
                                    }).ToList();
                                if (performances.Any())
                                {
                                    commandResult = string.Join(", ", values: performances);
                                }
                                else
                                {
                                    commandResult = "No performances";
                                }
                                break;
                            default:
                                commandResult = "Invalid command!";
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        commandResult = "Error: " + ex.Message;
                    }

                    Console.WriteLine(commandResult);
                }
            }
        }
    }
}