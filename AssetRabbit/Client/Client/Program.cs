using Client.Domain;
using Client.Helper;
using Client.Model;
using Client.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHibernate;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime;
using System.Text;

namespace Client
{
    class Program
    {
        public static void Main(string[] args)
        {
            String QueueName = "smartcattle_EquipmentQueue";
            ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost"};
            Program _program = new Program();
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;

            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: QueueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    Console.WriteLine("----------------------------Start Receive----------------------------");
                    byte[] body = ea.Body;
                    String message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(message);
                    //String type = _program.ParseMessage(message);
                    Boolean f_save = _program.SaveMessage(message, "");
                    if (f_save)
                    {
                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    }
                    long memory = GC.GetTotalMemory(true);
                    if (memory > 80000000)
                    {
                        Process.Start("Client.exe");
                        Environment.Exit(0);
                    }
                    Console.WriteLine("---------------------------------MEM---------------------------------");
                    Console.WriteLine(memory);
                    Console.WriteLine("---------------------------------MEM---------------------------------");
                };

                channel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }

        private Boolean SaveMessage(string message, string type)
        {
            Boolean retValue = true;
            Console.WriteLine("*****************************Equipment**********************************");
            Console.WriteLine(message);
            Console.WriteLine("************************************************************************");
            try
            {
                if(message.Substring(0, 1) != "[")
                {
                    message = "[" + message + "]";
                }
                List<EquipmentData> _EquipmentData = JsonConvert.DeserializeObject<List<EquipmentData>>(message);
                ISessionFactory _sessionEquipmentTemp = FluentNHibernateHelper.SensorTbl_Session();
                ISession sessionEquipmentTemp = _sessionEquipmentTemp.OpenSession();
                sessionEquipmentTemp.Clear();

                foreach (var item in _EquipmentData)
                {
                    sessionEquipmentTemp.Clear();
                    IList<EquipmentTbl> _EquipmentTbl = sessionEquipmentTemp.QueryOver<EquipmentTbl>().Where(x => x.Mac == item.Mac).List();
                    if (_EquipmentTbl.Count == 0)
                    {
                        EquipmentTbl newEquipment = new EquipmentTbl()
                        {
                            DeviceCategory = item.DeviceCategory,
                            projectName = item.projectName,
                            subId = item.spId,
                            DeviceType = item.DeviceType,
                            DeviceSubtype = item.DeviceSubtype,
                            PacketType = item.PacketType,
                            Version = item.Version,
                            PowerType = item.PowerType,
                            Equipmentid = item.Equipmentid,
                            Mac = item.Mac,
                            Projectid = item.Projectid,
                            Subprojectid = item.Subprojectid,
                            Zoneid = item.Zoneid,
                            Locationx = item.Locationx,
                            Locationy = item.Locationy,
                            Locationz = item.Locationz,
                            Date1 = item.Date1,
                            Date2 = item.Date2,
                            Reserved1 = item.Reserved1
                        };
                        sessionEquipmentTemp.Save(newEquipment);

                        if(item.spId == -1)
                        {
                            String RemoveSystemRole_Farm = string.Format("DELETE FROM {0} where MacAddress = '{1}'", "SmartCattle.SensorTbl", item.Mac);
                            sessionEquipmentTemp.CreateSQLQuery(RemoveSystemRole_Farm).ExecuteUpdate();

                            String RemoveEnvSensors = string.Format("DELETE FROM {0} where MAC = '{1}'", "SmartCattle.EnvSensors", item.Mac);
                            sessionEquipmentTemp.CreateSQLQuery(RemoveEnvSensors).ExecuteUpdate();
                        }
                        else
                        {
                            if (item.DeviceType.Equals("SensorTag"))
                            {
                                sessionEquipmentTemp.Clear();
                                IList<FarmTbl> FarmId = sessionEquipmentTemp.QueryOver<FarmTbl>().Where(x => x.SubprojectID == item.spId).List<FarmTbl>();
                                if (FarmId.Count != 0)
                                {
                                    SensorTbl newSensor = new SensorTbl()
                                    {
                                        cattleId = 0,
                                        FarmID = FarmId[0].ID,
                                        MacAddress = item.Mac
                                    };
                                    sessionEquipmentTemp.Save(newSensor);
                                }
                            }
                            else if (item.DeviceType.Equals("EnvironmentSensor"))
                            {
                                sessionEquipmentTemp.Clear();
                                IList<FarmTbl> FarmId = sessionEquipmentTemp.QueryOver<FarmTbl>().Where(x => x.SubprojectID == item.spId).List<FarmTbl>();
                                if (FarmId.Count != 0)
                                {
                                    EnvSensors newSensor = new EnvSensors()
                                    {
                                        FreeStallId = Convert.ToInt32(item.Zoneid),
                                        FarmId = FarmId[0].ID,
                                        Lat = Convert.ToDouble(item.Locationx),
                                        Lng = Convert.ToDouble(item.Locationy),
                                        MAC = item.Mac
                                    };
                                    sessionEquipmentTemp.Save(newSensor);
                                }
                            }
                        }
                    }
                    else
                    {
                        _EquipmentTbl[0].DeviceCategory = item.DeviceCategory;
                        _EquipmentTbl[0].projectName = item.projectName;
                        _EquipmentTbl[0].subId = item.spId;
                        _EquipmentTbl[0].DeviceType = item.DeviceType;
                        _EquipmentTbl[0].DeviceSubtype = item.DeviceSubtype;
                        _EquipmentTbl[0].PacketType = item.PacketType;
                        _EquipmentTbl[0].Version = item.Version;
                        _EquipmentTbl[0].PowerType = item.PowerType;
                        _EquipmentTbl[0].Equipmentid = item.Equipmentid;
                        _EquipmentTbl[0].Mac = item.Mac;
                        _EquipmentTbl[0].Projectid = item.Projectid;
                        _EquipmentTbl[0].Subprojectid = item.Subprojectid;
                        _EquipmentTbl[0].Zoneid = item.Zoneid;
                        _EquipmentTbl[0].Locationx = item.Locationx;
                        _EquipmentTbl[0].Locationy = item.Locationy;
                        _EquipmentTbl[0].Locationz = item.Locationz;
                        _EquipmentTbl[0].Date1 = item.Date1;
                        _EquipmentTbl[0].Date2 = item.Date2;
                        _EquipmentTbl[0].Reserved1 = item.Reserved1;

                        sessionEquipmentTemp.Update(_EquipmentTbl[0]);
                        sessionEquipmentTemp.Flush();

                        sessionEquipmentTemp.Clear();

                        if (item.spId == -1)
                        {
                            String RemoveSystemRole_Farm = string.Format("DELETE FROM {0} where MacAddress = '{1}'", "SmartCattle.SensorTbl", item.Mac);
                            sessionEquipmentTemp.CreateSQLQuery(RemoveSystemRole_Farm).ExecuteUpdate();

                            String RemoveEnvSensors = string.Format("DELETE FROM {0} where MAC = '{1}'", "SmartCattle.EnvSensors", item.Mac);
                            sessionEquipmentTemp.CreateSQLQuery(RemoveEnvSensors).ExecuteUpdate();
                        }
                        else
                        {
                            if (item.DeviceType.Equals("SensorTag"))
                            {
                                IList<FarmTbl> FarmId = sessionEquipmentTemp.QueryOver<FarmTbl>().Where(x => x.SubprojectID == item.spId).List<FarmTbl>();
                                if (FarmId.Count != 0)
                                {
                                    IList<SensorTbl> CurrentSensor = sessionEquipmentTemp.QueryOver<SensorTbl>().Where(x => x.MacAddress == item.Mac).List();
                                    if (CurrentSensor.Count != 0)
                                    {
                                        CurrentSensor[0].cattleId = 0;
                                        CurrentSensor[0].FarmID = FarmId[0].ID;
                                        CurrentSensor[0].MacAddress = item.Mac;
                                        sessionEquipmentTemp.Update(CurrentSensor[0]);
                                        sessionEquipmentTemp.Flush();
                                    }
                                    else
                                    {
                                        SensorTbl newSensor = new SensorTbl()
                                        {
                                            cattleId = 0,
                                            FarmID = FarmId[0].ID,
                                            MacAddress = item.Mac
                                        };
                                        sessionEquipmentTemp.Save(newSensor);
                                    }
                                }
                                else
                                {

                                }
                            }
                            else if (item.DeviceType.Equals("EnvironmentSensor"))
                            {
                                IList<FarmTbl> FarmId = sessionEquipmentTemp.QueryOver<FarmTbl>().Where(x => x.SubprojectID == item.spId).List<FarmTbl>();
                                if (FarmId.Count != 0)
                                {
                                    IList<EnvSensors> CurrentSensor = sessionEquipmentTemp.QueryOver<EnvSensors>().Where(x => x.MAC == item.Mac).List();
                                    if (CurrentSensor.Count != 0)
                                    {
                                        CurrentSensor[0].FreeStallId = Convert.ToInt32(item.Zoneid);
                                        CurrentSensor[0].FarmId = FarmId[0].ID;
                                        CurrentSensor[0].Lat = Convert.ToDouble(item.Locationx);
                                        CurrentSensor[0].Lng = Convert.ToDouble(item.Locationy);
                                        CurrentSensor[0].MAC = item.Mac;
                                        sessionEquipmentTemp.Update(CurrentSensor[0]);
                                        sessionEquipmentTemp.Flush();
                                    }
                                    else
                                    {
                                        EnvSensors newSensor = new EnvSensors()
                                        {
                                            FreeStallId = Convert.ToInt32(item.Zoneid),
                                            FarmId = FarmId[0].ID,
                                            Lat = Convert.ToDouble(item.Locationx),
                                            Lng = Convert.ToDouble(item.Locationy),
                                            MAC = item.Mac
                                        };
                                        sessionEquipmentTemp.Save(newSensor);
                                    }
                                }
                                else
                                {

                                }
                            }
                        }
                    }
                }

                sessionEquipmentTemp.Close();
                sessionEquipmentTemp.Dispose();
                _sessionEquipmentTemp.Close();
                _sessionEquipmentTemp.Dispose();
                Console.WriteLine("END*END*END*END*END*END********Equipment********END*END*END*END*END*END");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Equipment 102#####################################################################");
                Console.WriteLine(ex.Message);
                Console.WriteLine("----------------------------------------------------------------------------------");
                Console.WriteLine(ex.InnerException);
                Console.WriteLine("##################################################################################");
                retValue = false;
                WriteLog(ex.Message, "Equipment 102", message);
                retValue = true;
                //Process.Start("Client.exe");
                //Environment.Exit(0);
            }
            return retValue;
        }

        private String ParseMessage(string message)
        {
            String jsonTtype = JObject.Parse(message)["type"].ToString();
            return jsonTtype;
        }

        private void WriteLog(String Message, String Value, String input)
        {
            StreamWriter writer = new StreamWriter(@"LOGS/LOG_" + DateTime.Now.ToString("yyyy-mm-dd HH-MM-ss") + "_.txt");
            writer.WriteLine("MSG: " + Message + " - (" + Value + ") - " + DateTime.Now.ToString() + "\r\n\r\n" + input);
            writer.Close();
        }
    }
}
