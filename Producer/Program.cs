using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using System.Threading.Tasks;

namespace Producer
{
    class Send
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "192.168.99.100"};
            using (var con = factory.CreateConnection())
            using (var channel = con.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments:null);

                string message = "Hello World!";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                    routingKey: "hello",
                    basicProperties: null,
                    body: body);

                Console.WriteLine($"[x] Send {message}");
            }

            Console.WriteLine("Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
