using Confluent.Kafka;
using Newtonsoft.Json;
using Serilog;

namespace WebApiTools.Tools.Kafkas
{

    /// <summary>
    /// 接收端
    /// </summary>
    //public class Consumer<T> where T : class
    public class Consumer
    {

        /// <summary>
        /// KAFKA服务地址
        /// </summary>
        public string ServerAddress { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// 是否需要权限认证
        /// </summary>
        public bool IsAuthentication { get; set; } = false;

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 消费方名称
        /// </summary>
        public string ConsumerGroup { get; set; }

        /// <summary>
        ///  订阅
        /// </summary>
        /// <param name="dealMessage"></param>
        public void Subscribe(Action dealMessage)
        //public void Subscribe(Action<T> dealMessage)
        {
            ConsumerConfig config = null;

            if (IsAuthentication)
            {
                config = new ConsumerConfig
                {
                    GroupId = ConsumerGroup,
                    BootstrapServers = ServerAddress,
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                    SaslUsername = Username,
                    SaslPassword = Password,
                    SaslMechanism = SaslMechanism.Plain,
                    SecurityProtocol = SecurityProtocol.SaslPlaintext
                };
            }
            else
            {
                config = new ConsumerConfig
                {
                    GroupId = ConsumerGroup,
                    BootstrapServers = ServerAddress,
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                };
            }

            Task.Run(() =>
            {
                var builder = new ConsumerBuilder<string, string>(config);
                using (var consumer = builder.Build())
                {
                    consumer.Subscribe(Topic);
                    //成功订阅Kafka消息提示
                    Log.Information($"Kafka注册成功，Topic：{Topic}");

                    while (true)
                    {
                        var result = consumer.Consume();
                        try
                        {
                            ///  接收消息
                            var mes=result.Message.Value;

                            //var message = JsonConvert.DeserializeObject<T>(result.Message.Value);
                            //dealMessage(message);
                        }
                        catch (Exception ex)
                        {
                            Log.Error($"消费异常信息，Topic：{result.Topic}, 消息：{result.Message.Value}，异常详情：{ex.Message}");
                        }
                    }
                }
            });
        }

    }
}
