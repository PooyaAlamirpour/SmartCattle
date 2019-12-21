using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        private static String QueueName = "smartcattle_EquipmentQueue";
        private static IConnection connection;
        private static IModel channel;

        static void Main(string[] args)
        {
            try
            {
                var factory = new ConnectionFactory() { HostName = "127.0.0.1", Port = 5672 };
                connection = factory.CreateConnection();
                channel = connection.CreateModel();
                channel.QueueDeclare(queue: QueueName,
                                        durable: false,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);
            }
            catch (Exception ex)
            {
                String ack = ex.Message;
                Console.Write("Run C:\\Program Files\\RabbitMQ Server\\rabbitmq_server-3.7.7\\sbin\\rabbitmq-server.bat");
                Console.ReadKey();
            }
            
            while(true)
            {
                Thread.Sleep(50);
                SendMessage("{" +
                    "\"_id\":198," +
                    "\"DeviceCategory\":\"SD\"," +
                    "\"projectName\":\"smartcattle\"," +
                    "\"apiKey\":\"a43f6670-9d37-11e7-ad9d-819a9b28ee42\"," +
                    "\"spIdLabel\":\"مزرعه خیامی\"," +
                    "\"spId\":28," +
                    "\"stringMac\":\"a0e6f8df6d91\"," +
                    "\"DeviceType\":\"SensorTag\"," +
                    "\"DeviceSubtype\":\"Ear\"," +
                    "\"PacketType\":\"\"," +
                    "\"Version\":\"V1.0\"," +
                    "\"PowerType\":\"battery\"," +
                    "\"Equipmentid\":\"1010017\"," +
                    "\"Mac\":\"a0:e6:f8:df:6d:91\"," +
                    "\"Projectid\":\"\"," +
                    "\"Subprojectid\":\"\"," +
                    "\"Zoneid\":null," +
                    "\"Locationx\":null," +
                    "\"Locationy\":null," +
                    "\"Locationz\":null," +
                    "\"Date1\":\"28-Jul-18\"," +
                    "\"Date2\":\"\"," +
                    "\"Reserved1\":\"\"," +
                    "\"__v\":0}");
                //SendMessage("{\"packetType\":2,\"MAC\":\"cc:78:ab:6c:97:06\",\"detectorTime\":1522248986607,\"tObj\":24.8125,\"tAmb\":16.90625,\"spId\":8,\"type\":\"bodyTemperature\"}");
                //SendMessage("{\"MAC\":\"cc:78:ab:6c:97:06\",\"detectorTime\":1522248986607,\"activity\":{\"standing\":0,\"walking\":0,\"eating\":0,\"drinking\":0,\"sitting\":1,\"ruminating\":0},\"spId\":8,\"type\":\"activity\"}");
                //SendMessage("{\"MAC\":\"e3:ad:eb:31:e0:ef\",\"detectorTime\":1524301153830,\"dgnId\":\"b8:27:eb:36:b8:ba\",\"batteryLevel\":88,\"packetType\":5,\"humidity\":29.64,\"temperature\":25.05,\"subId\":8,\"uniqueKey\":\"e3:ad:eb:31:e0:ef1524301000000\",\"type\":\"ENV\"}");
            }
            

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        private static void SendMessage(String message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "",
                                 routingKey: QueueName,
                                 basicProperties: null,
                                 body: body);
            Console.WriteLine(" [x] Sent {0}", message);
        }
    }
}
