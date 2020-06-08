using AdminService.Events.Data;
using Contracts.MessageBus.Configuration;
using Contracts.MessageBus.Constants;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace AdminService.Events.EventHandlers.UserNotificationAdded
{
    public class UserNotificationAdded : IUserNotificationAdded
    {
        private RabbitMqConfig _publisherConfig = new RabbitMqConfig();

        public UserNotificationAdded(IOptions<PublisherConfig> publisherConfig)
        {
            RabbitMqConfig config = publisherConfig.Value.Queues.FirstOrDefault();

            _publisherConfig.Hostname = config.Hostname;
            _publisherConfig.QueueName = config.QueueName;
            _publisherConfig.UserName = config.UserName;
            _publisherConfig.Password = config.Password;
        }

        public void PublishUserNotification(NewUserNotification notification)
        {
            var factory = new ConnectionFactory() { HostName = _publisherConfig.Hostname, UserName = _publisherConfig.UserName, Password = _publisherConfig.Password };

            try
            {
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: _publisherConfig.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                    var json = JsonConvert.SerializeObject(notification);
                    var body = Encoding.UTF8.GetBytes(json);
                    IBasicProperties basicProperties = channel.CreateBasicProperties();

                    basicProperties.Type = EventTypes.UserNotificationAdded;

                    channel.BasicPublish(exchange: "", routingKey: _publisherConfig.QueueName, basicProperties: basicProperties, body: body);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
