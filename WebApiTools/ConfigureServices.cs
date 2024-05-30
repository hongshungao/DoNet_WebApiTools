using AgileConfig.Client;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using WebApiTools.Infrastructure;
using WebApiTools.TestService;
using WebApiTools.Tools.Event_Bus;

namespace WebApiTools
{
    public static class ConfigureServices
    {

        static readonly object _lock = new();//锁
        static readonly Type InterfaceEventSubscriber = typeof(IEventSubscriber<>);
        static IEnumerable<Type> _eventSubscribers = null!;
        /// <summary>
        /// 继承 IEventSubscriber的集合
        /// </summary>
        static IEnumerable<Type> EventSubscribers
        {
            get
            {
                lock (_lock)
                    return _eventSubscribers ??= Assemblies.InAllRequiredAssemblies.Where(x =>
                    !x.IsAbstract && x.IsPublic && x.IsClass && x.IsToGenericInterface(InterfaceEventSubscriber));
            }
        }

        /// <summary>
        /// 判断当前类型是否是指定类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="baseInterface"></param>
        /// <returns></returns>
        static bool IsToGenericInterface(this Type type, Type baseInterface)
        {
            if (type == null) return false;
            if (baseInterface == null) return false;
            bool result = type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == baseInterface);
            return result;
        }
        /// <summary>  
        /// 服务注册
        /// </summary>
        /// <param name="services"></param>
        public static void Configure(WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;
            var _env = builder.Environment.EnvironmentName;


           // builder.Services.AddScoped<Publisher>();
           // builder.Services.AddSingleton<ITestServices, TestServices>();
            // 配置自动注入eventBus
            //ConfigureEventBus(builder.Services);
            //ConfigureAgileConfig(configuration, _env);
        }

        #region Event_Bus
        public static void ConfigureEventBus(IServiceCollection services)
        {
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
        }


        #endregion
        #region AgileConfig
        public static void ConfigureAgileConfig(IConfigurationManager configurationManager,string env)
        {

            configurationManager.AddAgileConfig(z =>
            {
                // 根据launchSettings 自适应配置 apgileconfig的env
                z.ENV = env switch
                {
                    "Development" => "DEV",
                    "Staging" => "STG",
                    "Production" => "PROD",
                    _ => "TEST"
                }; ConfigClientOptions.FromConfiguration(configurationManager);
            });
        }

        #endregion

    }
}
