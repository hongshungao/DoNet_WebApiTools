using Confluent.Kafka;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WebApiTools.Tools.Kafkas
{
    internal class Producer
    {

        public static bool IsAuthentication = true;
        /// <summary>
        /// Kafka生产者
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static async Task<dynamic> SendAsync(string dto)
        {
            ProducerConfig config;
            var topic = "dd-lmes-stg-mam-process";
            if (!IsAuthentication)
            {
                config = new ProducerConfig()
                {
                    BootstrapServers = "",
                };
            }
            else
            {
                config = new ProducerConfig()
                {
                   //  kafka地址
                    BootstrapServers = "f2-kafka-01-stg.nioint.com:9092,f2-kafka-02-stg.nioint.com:9092,f2-kafka-03-stg.nioint.com:9092",
                     //kafka用户名
                    SaslUsername = "z5O7hG7DNem2v5Rs5kT",
                    // kafka密码
                    SaslPassword = "m1oaXD3H92V92CsxGjl",
                     //  配置认证机制
                    SaslMechanism = SaslMechanism.Plain,
                    SecurityProtocol = SecurityProtocol.SaslPlaintext,
                };
            }
            try
            {
                ProducerBuilder<Null, string> producerBuilder = new ProducerBuilder<Null, string>(config);

                //var setting = new JsonSerializerSettings
                //{
                //    //设置驼峰命名法
                //    ContractResolver = new CamelCasePropertyNamesContractResolver()
                //};

                var value = dto;//.ToLower();//JsonConvert.SerializeObject(dto, setting);
                using var p = producerBuilder.Build();

                var dr = await p.ProduceAsync(topic, new Message<Null, string> { Value = value });
                return new { Success = true, Message = dr.TopicPartition.Topic, Data = dr.Value };
            }
            catch (Exception ex)
            {
                return new { Success = false, Message = "错误：" + ex.ToString() };
            }
        }

    }
}

