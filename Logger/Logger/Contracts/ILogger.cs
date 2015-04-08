namespace Logger.Contracts
{
    public interface ILogger
    {
        string Warning(string message);

        string Info(string message);

        string Error(string message);
    }
}
