namespace Infrastructure
{
    public static class RabbitMQContants
    {
        // For dashboard => https://buck.rmq.cloudamqp.com
        // For info => https://www.rabbitmq.com/dotnet-api-guide.html

        public const string Connection = "amqp://voegahrb:3hluie1UwA94RJMdEaT1IG128RuoTLr6@buck.rmq.cloudamqp.com/voegahrb";
        public const string QueueName_Basic = "hello_world_queue";
        public const string QueueNameDirect = "direct_queue";
        public const string QueueNameTopic = "topic_queue";


        public const string ExchangeNameDirect = "direct_exchange";
        public const string ExchangeNameTopic = "topic_exchange";


        public const string RoutingNameDirect = "direct_routing";
        public const string RoutingNameTopic = "topic.routing.*";


    }
}