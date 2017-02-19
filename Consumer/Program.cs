using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer
{
    class Receive
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() {HostName = "192.168.99.100" };
            using (var con = factory.CreateConnection())
            using (var channel = con.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                arguments:null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($"[x] Received {message}");
                };

                channel.BasicConsume(
                    queue: "hello",
                    noAck: true,
                    consumer: consumer);

                Console.WriteLine("Press [enter] to exit");
                Console.ReadLine();
            }
        }
    }
}
