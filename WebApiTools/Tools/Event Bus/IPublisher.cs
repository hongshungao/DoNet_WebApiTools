namespace WebApiTools.Tools.Event_Bus
{
    public interface IPublisher
    {
        /// <summary>
        /// Event Publish
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="event">Event</param>
        /// <returns></returns>
        Task PublishAsync<T>(T @event, CancellationToken cancellationToken) where T : IEvent;
    }
}
