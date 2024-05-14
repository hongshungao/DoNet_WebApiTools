using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiTools.Tools.Event_Bus
{
    public interface IEventSubscriber<T> where T : IEvent
    {
        Task HandleAsync(T @event, CancellationToken ct);

        /// <summary>
        /// 执行排序
        /// </summary>
        int Index { get; }

        /// <summary>
        /// 如果发生错误是否抛出异常,将阻塞后续Handler
        /// </summary>
        bool ThrowIfError { get; }
    }
    public abstract class EventSubscriber<T> : IEventSubscriber<T> where T : IEvent
    {
        /// <summary>
        /// 用于测试样例
        /// </summary>
        /// <param name="event"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public abstract Task HandleAsync(T @event, CancellationToken ct);
        

        public virtual int Index => 0;
        /// <summary>
        /// 默认不抛出异常
        /// </summary>
        public virtual bool ThrowIfError => false;
    }
}
