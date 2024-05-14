using WebApiTools.Infrastructure;
using WebApiTools.TestService;
using WebApiTools.Tools.Event_Bus;

namespace WebApiTools
{
    public static class ConfigureServices
    {
        /// <summary>
        /// 服务注册
        /// </summary>
        /// <param name="services"></param>
        public static void Configure(IServiceCollection services)
        {
            ConfigureEventBus(services);

        }

        #region Event_Bus
        public static void ConfigureEventBus(IServiceCollection services)
        {
            #region 测试EventBus 

            services.AddSingleton<ITestServices, TestServices>();
            services.AddScoped<Publisher>();

            //注册EventHanders
            foreach (var subscriberType in EventSubscribers)
            {
                //存在一个订阅者订阅多个事件的情况:
                var baseTypes = subscriberType.GetInterfaces().Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == InterfaceEventSubscriber).ToArray();
                foreach (var baseType in baseTypes)
                {
                    services.AddScoped(baseType, subscriberType);

                }
            }
            #endregion

        }


        #endregion
        static readonly object _lock = new();//锁
        static readonly Type InterfaceEventSubscriber = typeof(IEventSubscriber<>);
        static IEnumerable<Type> _eventSubscribers = null!;
        static IEnumerable<Type> EventSubscribers
        {
            get
            {
                lock (_lock)
                    return _eventSubscribers ??= Assemblies.InAllRequiredAssemblies.Where(x =>
                    !x.IsAbstract && x.IsPublic && x.IsClass && x.IsToGenericInterface(InterfaceEventSubscriber));
            }
        }
        static bool IsToGenericInterface(this Type type, Type baseInterface)
        {
            if (type == null) return false;
            if (baseInterface == null) return false;
            bool result = type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == baseInterface);
            return result;
        }

    }
}
