namespace BtcTurk.Order.Api.MessageBrokers;

public interface IConsumer
{
    public Task InitializeAsync();
}