using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading;

namespace Consumer
{
    class ReceiveLogs
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() {HostName = "192.168.99.100" };
            using (var con = factory.CreateConnection())
            using (var channel = con.CreateModel())
            {
                channel.ExchangeDeclare(exchange:"logs",type:"fanout");
                var queueName = channel.QueueDeclare().QueueName;
                channel.QueueBind(queue: queueName, exchange: "logs", routingKey: "");
                
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($"[x] {message}");

                    int dots = message.Split('.').Length - 1;
                    Thread.Sleep(dots * 1000);
                    
                };

                channel.BasicConsume(
                    queue: queueName,
                    noAck: true,
                    consumer: consumer);

                Console.WriteLine("Press [enter] to exit");
                Console.ReadLine();
            }
        }
    }
}
