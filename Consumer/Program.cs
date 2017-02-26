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
                channel.ExchangeDeclare(exchange:"topic_logs",type:"topic");
                var queueName = channel.QueueDeclare().QueueName;

                if(args.Length < 1)
                {
                    Console.Error.WriteLine($"Usage {Environment.GetCommandLineArgs()[0]} [binding_key]");
                    Console.ReadLine();
                    Environment.ExitCode = 1;
                    return;
                }

                foreach(var bindingkey in args)
                {
                    channel.QueueBind(queue: queueName, exchange: "topic_logs", routingKey: bindingkey);
                }

                Console.WriteLine(" [*] Waiting for messaages.");                
                
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);

                    var routingKey = ea.RoutingKey;
                    Console.WriteLine($"[x] Received {routingKey} {message}");
                                        
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
