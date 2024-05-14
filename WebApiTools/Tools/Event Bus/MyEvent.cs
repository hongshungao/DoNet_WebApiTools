using Microsoft.AspNetCore.Mvc;

namespace WebApiTools.Tools.Event_Bus
{
    public class MyEvent:IEvent
    {
        [FromQuery]
        public string? Message { get; set; }
    }
    public class MyEvent2 : IEvent
    {
        public string? Message { get; set; }
    }


    [EventSubscriber(Index = 0, IsAsync = true)]
    public class MyEventHandler : EventSubscriber<MyEvent>
    {
        private readonly ILogger<MyEventHandler> _logger;
        public MyEventHandler(ILogger<MyEventHandler> logger)
        {
            _logger = logger;
        }

        public override async Task HandleAsync(MyEvent @event, CancellationToken ct)
        {
            //模拟异步操作
            await Task.Delay(5000, ct);

            _logger.LogInformation($"msg 2 : {@event.Message}");
        }
    }

    /// <summary>
    /// 更早执行的Handler
    /// </summary>
    [EventSubscriber(Index = 1)]
    public class MyEventHandler2 : EventSubscriber<MyEvent>
    {
        private readonly ILogger<MyEventHandler2> _logger;
        public MyEventHandler2(ILogger<MyEventHandler2> logger)
        {
            _logger = logger;
        }

        public override Task HandleAsync(MyEvent @event, CancellationToken ct)
        {
            _logger.LogInformation($"msg 1 : {@event.Message}");
            return Task.CompletedTask;
        }

    }

    /// <summary>
    /// 抛出异常的Handler
    /// </summary>
    [EventSubscriber(Index = -2, ThrowIfError = false)]
    public class MyEventHandler3 : EventSubscriber<MyEvent>
    {
        private readonly ILogger<MyEventHandler3> _logger;
        public MyEventHandler3(ILogger<MyEventHandler3> logger)
        {
            _logger = logger;
        }

        public override Task HandleAsync(MyEvent @event, CancellationToken ct)
        {
            //throw new Exception("error");
            _logger.LogInformation($"muti msg : {@event.Message}");
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// 同时订阅多个事件
    /// </summary>
    [EventSubscriber(IsAsync = true, Index = 0, ThrowIfError = false)]
    public class MyEventHandler4 : IEventSubscriber<MyEvent>, IEventSubscriber<MyEvent2>
    {
        private readonly ILogger<MyEventHandler4> _logger;
        public MyEventHandler4(ILogger<MyEventHandler4> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(MyEvent @event, CancellationToken ct)
        {
            _logger.LogInformation($"muti msg 1 : {@event.Message}");
            return Task.CompletedTask;
        }

        public Task HandleAsync(MyEvent2 @event, CancellationToken ct)
        {
            _logger.LogInformation($"muti msg 2 : {@event.Message}");
            return Task.CompletedTask;
        }
         
        public int Index { get; }
        public bool ThrowIfError { get; } = false;
    }
    }
