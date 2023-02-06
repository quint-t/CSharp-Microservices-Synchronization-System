namespace SpecProfiles.Services
{
    public class EventProvider
    {
        public delegate void NewEvent(Event @event);
        public event NewEvent? Notify;

        public void NotifySubscribers(Event @event)
        {
            Task.Run(() => {
                this.Notify?.Invoke(@event);
            });
        }
    }
}
