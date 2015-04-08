namespace Logger.Appenders
{
    using Contracts;

    public class ConsoleAppender : Appender
    {
        public ConsoleAppender(ILayout layout) 
            : base(layout)
        {
        }
    }
}
