using Infrastructure;
using Infrastructure.Core;
using RabbitMQ.Client;
using System;
using System.Text;

namespace Publisher
{
    class Producer
    {
        static void Main(string[] args)
        {
            //BasicWay();
            // DirectExchange();

            TopicExchange();
        }

        private static void BasicWay()
        {
            // Similar to DirectExchange, the routing key should match the queue name.
            RabbitMQCore mqSettings = new RabbitMQCore();
            IModel channel = mqSettings.connection.CreateModel();

            string message = "Hello world to rabbitMQ, by Linoy";
            byte[] formattedMsg = Encoding.UTF8.GetBytes(message);

            channel.QueueDeclare(RabbitMQContants.QueueName_Basic, false, false, false, null);
            channel.BasicPublish(string.Empty, RabbitMQContants.QueueName_Basic, null, formattedMsg);

            Console.WriteLine("Message published successfully!!");
            Console.ReadKey();
            channel.Close();
        }

        private static void DirectExchange() // Similar behaviour of Basic
        {
            #region CONNECTION AND CHANNEL

            RabbitMQCore mqSettings = new RabbitMQCore();
            IModel channel = mqSettings.connection.CreateModel();

            #endregion

            while (Console.ReadKey().Key != ConsoleKey.Escape)
            {
                #region MESSAGE

                string message = Console.ReadLine();
                byte[] formattedMsg = Encoding.UTF8.GetBytes(message);

                #endregion

                #region EXCHANGE DECLARE

                channel.ExchangeDeclare(RabbitMQContants.ExchangeNameDirect, ExchangeType.Direct);

                #endregion

                channel.BasicPublish(RabbitMQContants.ExchangeNameDirect, RabbitMQContants.RoutingNameDirect, null, formattedMsg);

                Console.WriteLine("Message published successfully!!");
            }

            Console.ReadKey();
            channel.Close();
        }

        private static void TopicExchange()
        {
            #region CONNECTION AND CHANNEL

            RabbitMQCore mqSettings = new RabbitMQCore();
            IModel channel = mqSettings.connection.CreateModel();

            #endregion

            Console.WriteLine("TOPIC EXCHANGE!!");

            while (Console.ReadKey().Key != ConsoleKey.Escape)
            {
                #region MESSAGE

                string message = Console.ReadLine();
                byte[] formattedMsg = Encoding.UTF8.GetBytes(message);

                #endregion

                #region EXCHANGE DECLARE

                channel.ExchangeDeclare(RabbitMQContants.ExchangeNameTopic, ExchangeType.Topic);

                #endregion

                #region PUBLISH MESSAGE

                IBasicProperties properties = channel.CreateBasicProperties();
                properties.ContentType = "text/plain";
                properties.Expiration = "360000";

                #region HANDLING UNROUTED MESSAGES

                channel.BasicReturn += Channel_BasicReturn;

                #endregion

                channel.BasicPublish(RabbitMQContants.ExchangeNameTopic, "topic.routing.firstword", true, properties, formattedMsg);

                #endregion

                Console.WriteLine("Message published successfully!!");
            }

            Console.ReadKey();
            channel.Close();
        }

        private static void Channel_BasicReturn(object sender, RabbitMQ.Client.Events.BasicReturnEventArgs args)
        {
            // The messages which is published with mandatory flag to 'true', 
            // but cannot be delivered(Eg: Not bound to queue) then the broker will return it to sending client. 

            byte[] body = args.Body;
            string message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"The message '{message}' is not delivered to consumer with a replay {args.ReplyText}");
        }
    }
}
