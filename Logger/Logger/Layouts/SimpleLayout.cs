namespace Logger.Layouts
{
    using System;
    using Contracts;

    public class SimpleLayout : ILayout
    {
        public DateTime DateTime { get; set; }

        // TODO: report level property

        public string Message { get; set; }

        // TODO: edit string format
        public string Format()
        {
            return string.Format("<date-time> - <report level> - <message>");
        }
    }
}
