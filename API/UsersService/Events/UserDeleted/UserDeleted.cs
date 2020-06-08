﻿using Contracts.MessageBus.Configuration;
using Contracts.MessageBus.Constants;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;
using UsersService.Events.Data;

namespace UsersService.Events
{
    public class UserDeleted : IUserDeleted
    {
        private RabbitMqConfig _publisherConfig = new RabbitMqConfig();

        public UserDeleted(IOptions<PublisherConfig> publisherConfig)
        {
            RabbitMqConfig config = publisherConfig.Value.Queues.FirstOrDefault();

            _publisherConfig.Hostname = config.Hostname;
            _publisherConfig.QueueName = config.QueueName;
            _publisherConfig.UserName = config.UserName;
            _publisherConfig.Password = config.Password;
        }

        public void PublishDeletedUser(DeletedUser user)
        {
            var factory = new ConnectionFactory() { HostName = _publisherConfig.Hostname, UserName = _publisherConfig.UserName, Password = _publisherConfig.Password };

            try
            {

                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: _publisherConfig.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                    var json = JsonConvert.SerializeObject(user);
                    var body = Encoding.UTF8.GetBytes(json);
                    IBasicProperties basicProperties = channel.CreateBasicProperties();

                    basicProperties.Type = EventTypes.UserDeleted;

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
