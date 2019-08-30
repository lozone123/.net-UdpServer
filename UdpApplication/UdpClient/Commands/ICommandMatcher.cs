namespace UdpClient.Commands
{
    public interface ICommandMatcher
    {
        ICommand Match(string cmd);
    }
}