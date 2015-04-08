namespace Logger.Appenders
{
    using Contracts;

    public abstract class Appender : IAppender
    {
        public Appender(ILayout layout)
        {
            this.Layout = layout;
        }

        public ILayout Layout { get; set; }
    }
}
