using AdminService.Events.Data;
using AdminService.Events.EventHandlers.Base;
using AdminService.Events.EventHandlers.UserAdded;
using AdminService.Events.EventHandlers.UserDeleted;
using AdminService.Events.EventHandlers.UserModified;
using Contracts.MessageBus;
using Contracts.MessageBus.Configuration;
using Contracts.MessageBus.Constants;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdminService.Events
{
    public class SubscriberService : BackgroundService
    {
        private IModel _channel;
        private IConnection _connection;
        private IServiceProvider _serviceProvider;

        private RabbitMqConfig _subscriberConfig = new RabbitMqConfig();

        public SubscriberService(IOptions<SubscriberConfig> subscriberConfig, IServiceProvider serviceProvider)
        {
            RabbitMqConfig config = subscriberConfig.Value.Queues.FirstOrDefault();
            _subscriberConfig.Hostname = config.Hostname;
            _subscriberConfig.QueueName = config.QueueName;
            _subscriberConfig.UserName = config.UserName;
            _subscriberConfig.Password = config.Password;
            _serviceProvider = serviceProvider;
            InitializeRabbitMqListener();
        }

        private void InitializeRabbitMqListener()
        {
            var factory = new ConnectionFactory
            {
                HostName = _subscriberConfig.Hostname,
                UserName = _subscriberConfig.UserName,
                Password = _subscriberConfig.Password
            };

            _connection = factory.CreateConnection();
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _subscriberConfig.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                using (var scope = _serviceProvider.CreateScope())
                {
                    IBaseEvent ev = FetchEvent(ea.BasicProperties.Type, scope);
                    if (ev != null)
                    {
                        ev.HandleEvent(content);
                    }
                }
                _channel.BasicAck(ea.DeliveryTag, false);
            };
            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerConsumerCancelled;

            _channel.BasicConsume(_subscriberConfig.QueueName, false, consumer);

            return Task.CompletedTask;
        }

        private IBaseEvent FetchEvent(string eventType, IServiceScope scope)
        {
            return eventType switch
            {
                EventTypes.UserAdded => scope.ServiceProvider.GetRequiredService<IUserAdded>(),
                EventTypes.UserModified => scope.ServiceProvider.GetRequiredService<IUserModified>(),
                EventTypes.UserDeleted => scope.ServiceProvider.GetRequiredService<IUserDeleted>(),
                _ => null
            };
        }

        private void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerRegistered(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerShutdown(object sender, ShutdownEventArgs e)
        {
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}