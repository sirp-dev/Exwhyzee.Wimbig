namespace Exwhyzee.Events
{
    public interface INotifyProxy
    {
        IEventBus EventBus { get; set; }
    }
}
