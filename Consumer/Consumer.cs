using Infrastructure;
using Infrastructure.Core;
using RabbitMQ.Client;
using System;
using System.Text;

namespace Consumer
{
    class Consumer
    {
        static void Main(string[] args)
        {
            //BasicWay();

            //DirectExchange();

            TopicExchange();
        }

        private static void BasicWay()
        {
            RabbitMQCore mqSettings = new RabbitMQCore();
            IModel channel = mqSettings.connection.CreateModel();

            channel.QueueDeclare(RabbitMQContants.QueueName_Basic, false, false, true, null);
            BasicGetResult result = mqSettings.GetPullRequest(channel, RabbitMQContants.QueueName_Basic, true);

            if (result == null)
            {
                Console.WriteLine("No message from RabbitMQ");
            }
            else
            {
                byte[] responseBody = result.Body;
                string message = Encoding.UTF8.GetString(responseBody);
                Console.WriteLine("Message from RabbitMQ :" + message);

            }

            Console.WriteLine("Listened the pull request");
            Console.ReadKey();
            channel.Close();
        }

        private static void DirectExchange()
        {
            RabbitMQCore mqSettings = new RabbitMQCore();
            IModel channel = mqSettings.connection.CreateModel();

            channel.QueueDeclare(RabbitMQContants.QueueNameDirect, false, false, true, null);
            channel.ExchangeDeclare(RabbitMQContants.ExchangeNameDirect, ExchangeType.Direct);
            channel.QueueBind(RabbitMQContants.QueueNameDirect, RabbitMQContants.ExchangeNameDirect, RabbitMQContants.RoutingNameDirect);

            mqSettings.MakePushRequest(channel, RabbitMQContants.QueueNameDirect, false);

            Console.WriteLine("Listening the queue using push request");
            Console.ReadKey();
            channel.Close();
        }

        private static void TopicExchange()
        {
            RabbitMQCore mqSettings = new RabbitMQCore();
            IModel channel = mqSettings.connection.CreateModel();

            channel.QueueDeclare(RabbitMQContants.QueueNameTopic, false, false, true, null);
            channel.ExchangeDeclare(RabbitMQContants.ExchangeNameDirect, ExchangeType.Direct);
            channel.QueueBind(RabbitMQContants.QueueNameTopic, RabbitMQContants.ExchangeNameTopic, RabbitMQContants.RoutingNameTopic);

            mqSettings.MakePushRequest(channel, RabbitMQContants.QueueNameTopic, false);

            Console.WriteLine("Listening the queue using push request");
            Console.ReadKey();
            channel.Close();
        }
    }
}
