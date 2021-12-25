using System;
using System.Text;
using Microsoft.AspNet.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace JobsityChat
{
    public static class RabbitMQ
    {
        public static void Publish(string hostname, string username, string password, string message)
        {
            var factory = new ConnectionFactory() { HostName = hostname };
            if(username != "" && password != "")
            {
                factory.UserName = username;
                factory.Password = password;
            }
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "chat",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "chat",
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine(" [x] Sent {0}", message);
            }
        }

        public static void StartConsumer(string hostname, string username, string password)
        {
            var factory = new ConnectionFactory() { HostName = hostname };
            if (username != "" && password != "")
            {
                factory.UserName = username;
                factory.Password = password;
            }
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.QueueDeclare(queue: "chat",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var context = GlobalHost.ConnectionManager.GetHubContext<JobsityHub>();
                context.Clients.All.addNewMessageToPage("Bot", message);
            };
            channel.BasicConsume(queue: "chat", autoAck: true, consumer: consumer);
        }
    }
}