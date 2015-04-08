namespace Logger.Appenders
{
    using Contracts;

    public class FileAppender : Appender
    {
        public FileAppender(ILayout layout) 
            : base(layout)
        {
        }
    }
}
