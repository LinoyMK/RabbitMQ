using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Infrastructure.Core
{
    public class RabbitMQCore : IDisposable
    {
        public readonly IConnection connection;
        private string pushMessage { get; set; }

        public RabbitMQCore()
        {
            connection = GetConnection();
        }

        private IConnection GetConnection()
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.Uri = RabbitMQContants.Connection;

            #region AUTOMATIC RECOVERY FROM CONNECTION FAILURES

            factory.AutomaticRecoveryEnabled = true;
            factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(10);

            #endregion

            IConnection connection = factory.CreateConnection();
            return connection;
        }

        public BasicGetResult GetPullRequest(IModel channel, string queueName, bool noAck)
        {
            return channel.BasicGet(queueName, noAck);
        }

        public void MakePushRequest(IModel channel, string queueName, bool noAck)
        {
            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

            consumer.Received += (ch, ea) =>
            {
                byte[] body = ea.Body;
                pushMessage = Encoding.UTF8.GetString(body);
                Console.WriteLine("Message received.." + pushMessage);

                IBasicProperties msgProp = ea.BasicProperties;

                if (msgProp != null)
                {
                    Console.WriteLine("Message properties : Expiration " + msgProp.Expiration);
                    Console.WriteLine("Message properties : ContentType " + msgProp.ContentType);
                }


                channel.BasicAck(ea.DeliveryTag, false);
            };

            string consumerTag = channel.BasicConsume(queueName, noAck, consumer);

            // channel.BasicCancel(consumerTag); //--> To cancel the active consumer...
        }

        public void Dispose()
        {
            connection.Close();
        }
    }
}
