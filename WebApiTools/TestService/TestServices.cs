
using WebApiTools.Tools.Event_Bus;

namespace WebApiTools.TestService
{
    /// <summary>
    /// 测试
    /// </summary>
    /// <param name="ServiceProvider">sp</param>
    public class TestServices(IServiceProvider ServiceProvider) : ITestServices
    {
        public virtual async Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : IEvent
        {
            using var scope = ServiceProvider.CreateScope();
            var publisher = scope.ServiceProvider.GetRequiredService<Publisher>();
            await publisher.PublishAsync(@event, cancellationToken);
        }
    }
}
