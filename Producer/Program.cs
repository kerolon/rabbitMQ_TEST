using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using System.Threading.Tasks;

namespace Producer
{
    class EmitLog
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "192.168.99.100"};
            using (var con = factory.CreateConnection())
            using (var channel = con.CreateModel())
            {
                channel.ExchangeDeclare("logs", "fanout");
                
                string message = GetMessage(args);
                var body = Encoding.UTF8.GetBytes(message);
                
                channel.BasicPublish(exchange: "logs",
                    routingKey: "",
                    basicProperties: null,
                    body: body);

                Console.WriteLine($"[x] Send {message}");
            }

            Console.WriteLine("Press [enter] to exit.");
            Console.ReadLine();
        }

        private static string GetMessage(string[] args)
        {
            return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
        }
    }
}
