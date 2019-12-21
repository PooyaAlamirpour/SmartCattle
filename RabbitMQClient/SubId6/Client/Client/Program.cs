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
using System.Threading;

namespace Client
{
    class Program
    {
        private static int SubprojectID = 6;
        public static void Main(string[] args)
        {
            String QueueName = "smartcattle_DataQueue_" + SubprojectID.ToString();
            ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };
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
                    WriteLog3Hour(message);
                    try
                    {
                        Boolean f_save = false;
                        String type = _program.ParseMessage(message);
                        if (type.Equals("NaN"))
                        {
                            String[] MessageList = _program.SplitToArray(message);
                            foreach (var _message in MessageList)
                            {
                                type = _program.ParseMessage(_message, packetType: true);
                                _program.SaveMessage(_message, type);
                            }
                            f_save = true;
                        }
                        else
                        {
                            f_save = _program.SaveMessage(message, type);
                        }
                        if (f_save)
                        {
                            channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                        }
                    }
                    catch (Exception ex)
                    {
                        _program.WriteLog(ex.Message, "Start 000", message);
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
                    Thread.Sleep(100);
                };

                channel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }

        private Boolean SaveMessage(string message, string type)
        {
            Boolean retValue = true;
            switch (type)
            {
                case "bodyTemperature":
                    Console.WriteLine("**************************BodyTemperature*******************************");
                    Console.WriteLine(message);
                    Console.WriteLine("************************************************************************");
                    try
                    {
                        BodyTemperature bodyTemperature = JsonConvert.DeserializeObject<BodyTemperature>(message);
                        ISessionFactory _sessionBodyTemp = FluentNHibernateHelper.SensorTbl_Session();
                        ISession sessionBodyTemp = _sessionBodyTemp.OpenSession();
                        sessionBodyTemp.Clear();
                        int cattleIdBodyTemp = sessionBodyTemp.QueryOver<SensorTbl>().Where(x => x.MacAddress == bodyTemperature.MAC).Select(x => x.cattleId).SingleOrDefault<int>();
                        Console.WriteLine(cattleIdBodyTemp.ToString());
                        if (cattleIdBodyTemp != 0)
                        {
                            ISessionFactory _sessionTempreture = FluentNHibernateHelper.SensorTbl_Session();
                            using (ISession sessionTempreture = _sessionTempreture.OpenSession())
                            {
                                sessionTempreture.Clear();
                                DateTime detectorTime = Utils.UnixTimeStampToDateTime(bodyTemperature.detectorTime.ToString());
                                Console.WriteLine(detectorTime);
                                int FarmID = sessionTempreture.QueryOver<FarmTbl>().Where(x => x.SubprojectID == SubprojectID).Select(x => x.ID).SingleOrDefault<int>();
                                TempretureTbl TempretureObj = new TempretureTbl
                                {
                                    value = bodyTemperature.tObj,
                                    cattleId = cattleIdBodyTemp,
                                    date = detectorTime,
                                    LastRecievedId = 0,
                                    FarmID = FarmID,
                                    FreeStall = 5
                                };
                                try
                                {
                                    sessionTempreture.Save(TempretureObj);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("bodyTemperature 101###############################################################");
                                    Console.WriteLine(ex.Message);
                                    Console.WriteLine("----------------------------------------------------------------------------------");
                                    Console.WriteLine(ex.InnerException);
                                    Console.WriteLine("##################################################################################");
                                    WriteLog(ex.Message, "bodyTemperature 101", message);
                                    if (ex.Message.Contains("An invalid or incomplete configuration was used while creating a SessionFactory"))
                                    {
                                        retValue = false;
                                        Process.Start("Client.exe");
                                        Environment.Exit(0);
                                    }
                                    else
                                    {
                                        retValue = true;
                                    }
                                }
                                sessionTempreture.Close();
                                sessionTempreture.Dispose();
                                _sessionTempreture.Close();
                                _sessionTempreture.Dispose();
                            }
                        }
                        else
                        {
                            retValue = true;
                        }
                        sessionBodyTemp.Close();
                        sessionBodyTemp.Dispose();
                        _sessionBodyTemp.Close();
                        _sessionBodyTemp.Dispose();
                        Console.WriteLine("END*END*END*END*END*END***BodyTemperature********END*END*END*END*END*END");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("bodyTemperature 102###############################################################");
                        Console.WriteLine(ex.Message);
                        Console.WriteLine("----------------------------------------------------------------------------------");
                        Console.WriteLine(ex.InnerException);
                        Console.WriteLine("##################################################################################");
                        WriteLog(ex.Message, "bodyTemperature 102", message);
                        if (ex.Message.Contains("An invalid or incomplete configuration was used while creating a SessionFactory"))
                        {
                            retValue = false;
                            Process.Start("Client.exe");
                            Environment.Exit(0);
                        }
                        else
                        {
                            retValue = true;
                        }
                    }
                    
                    break;

                case "activity":
                    Console.WriteLine("******************************Activity**********************************");
                    Console.WriteLine(message);
                    Console.WriteLine("************************************************************************");
                    try
                    {
                        Activities activity = JsonConvert.DeserializeObject<Activities>(message);
                        ISessionFactory _sessionSensor = FluentNHibernateHelper.SensorTbl_Session();
                        ISession sessionSensor = _sessionSensor.OpenSession();
                        sessionSensor.Clear();
                        int cattleIdSensor = sessionSensor.QueryOver<SensorTbl>().Where(x => x.MacAddress == activity.MAC).Select(x => x.cattleId).SingleOrDefault<int>();
                        if (cattleIdSensor != 0)
                        {
                            double MaxActivityValue =
                                activity.activity.drinking +
                                activity.activity.eating +
                                activity.activity.ruminating +
                                activity.activity.sitting +
                                activity.activity.standing +
                                activity.activity.walking;

                            double Drinking = 0;
                            double Eating = 0;
                            double Rumination = 0;
                            double Sitting = 0;
                            double Standing = 0;
                            double Walking = 0;

                            if (MaxActivityValue != 0)
                            {
                                Drinking = ((activity.activity.drinking * 100) / MaxActivityValue);
                                Eating = ((activity.activity.eating * 100) / MaxActivityValue);
                                Rumination = ((activity.activity.ruminating * 100) / MaxActivityValue);
                                Sitting = ((activity.activity.sitting * 100) / MaxActivityValue);
                                Standing = ((activity.activity.standing * 100) / MaxActivityValue);
                                Walking = ((activity.activity.walking * 100) / MaxActivityValue);
                            }
                            if(activity.detectorTime == 0)
                            {
                                retValue = true;
                            }
                            else
                            {
                                DateTime detectorTime = Utils.UnixTimeStampToDateTime(activity.detectorTime.ToString());
                                ISessionFactory _sessionActivity = FluentNHibernateHelper.SensorTbl_Session();
                                using (ISession sessionActivity = _sessionActivity.OpenSession())
                                {
                                    sessionActivity.Clear();
                                    int FarmID = sessionActivity.QueryOver<FarmTbl>().Where(x => x.SubprojectID == SubprojectID).Select(x => x.ID).SingleOrDefault<int>();
                                    ActivityStateTbl ActivityObj = new ActivityStateTbl
                                    {
                                        Sitting = (decimal)Sitting,
                                        Standing = (decimal)Standing,
                                        Walking = (decimal)Walking,
                                        Eating = (decimal)Eating,
                                        Rumination = (decimal)Rumination,
                                        Drinking = (decimal)Drinking,
                                        cattleId = cattleIdSensor,
                                        date = detectorTime,
                                        FarmID = FarmID,
                                        LastRecievedId = 0
                                    };
                                    sessionActivity.Save(ActivityObj);
                                    sessionActivity.Close();
                                    sessionActivity.Dispose();
                                    _sessionActivity.Close();
                                    _sessionActivity.Dispose();

                                    retValue = true;
                                }
                            }
                           
                        }
                        else
                        {
                            retValue = true;
                        }
                        sessionSensor.Close();
                        sessionSensor.Dispose();
                        _sessionSensor.Close();
                        _sessionSensor.Dispose();
                        Console.WriteLine("------------------------------Activity-------------------------------");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Activity 103###############################################################");
                        Console.WriteLine(ex.Message);
                        Console.WriteLine("----------------------------------------------------------------------------------");
                        Console.WriteLine(ex.InnerException);
                        Console.WriteLine("###########################################################################");
                        WriteLog(ex.Message, "Activity 103", message);
                        if (ex.Message.Contains("An invalid or incomplete configuration was used while creating a SessionFactory"))
                        {
                            retValue = false;
                            Process.Start("Client.exe");
                            Environment.Exit(0);
                        }
                        else
                        {
                            retValue = true;
                        }
                    }
                    
                    break;

                case "position":
                    Console.WriteLine("******************************Position**********************************");
                    Console.WriteLine(message);
                    Console.WriteLine("************************************************************************");
                    try
                    {
                        Position position = JsonConvert.DeserializeObject<Position>(message);
                        ISessionFactory _session = FluentNHibernateHelper.SensorTbl_Session();
                        ISession session = _session.OpenSession();
                        session.Clear();
                        int cattleId = session.QueryOver<SensorTbl>().Where(x => x.MacAddress == position.MAC).Select(x => x.cattleId).SingleOrDefault<int>();
                        if (cattleId != 0)
                        {
                            ISessionFactory _sessionCattlePosition = FluentNHibernateHelper.SensorTbl_Session();
                            using (ISession sessionCattlePosition = _sessionCattlePosition.OpenSession())
                            {
                                sessionCattlePosition.Clear();
                                int FarmID = sessionCattlePosition.QueryOver<FarmTbl>().Where(x => x.SubprojectID == SubprojectID).Select(x => x.ID).SingleOrDefault<int>();
                                DateTime detectorTime = Utils.UnixTimeStampToDateTime(position.detectorTime.ToString());
                                CattlePositionTbl CattlePositionObj = new CattlePositionTbl
                                {
                                    Latitude = position.latitude,
                                    Longitude = position.longitude,
                                    cattleId = cattleId,
                                    date = detectorTime,
                                    LastRecievedId = 0,
                                    FarmID = FarmID,
                                    FreeStall = 5
                                };
                                sessionCattlePosition.Save(CattlePositionObj);
                                sessionCattlePosition.Close();
                                sessionCattlePosition.Dispose();
                                _sessionCattlePosition.Close();
                                _sessionCattlePosition.Dispose();
                            }
                        }
                        else
                        {
                            retValue = true;
                        }
                        session.Close();
                        session.Dispose();
                        _session.Close();
                        _session.Dispose();
                        Console.WriteLine("------------------------------Position-------------------------------");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Position 104###############################################################");
                        Console.WriteLine(ex.Message);
                        Console.WriteLine("----------------------------------------------------------------------------------");
                        Console.WriteLine(ex.InnerException);
                        Console.WriteLine("###########################################################################");
                        WriteLog(ex.Message, "Position 104", message);
                        if (ex.Message.Contains("An invalid or incomplete configuration was used while creating a SessionFactory"))
                        {
                            retValue = false;
                            Process.Start("Client.exe");
                            Environment.Exit(0);
                        }
                        else
                        {
                            retValue = true;
                        }
                    }
                    break;

                case "ENV":
                    Console.WriteLine("******************************Enviroment*******************************");
                    Console.WriteLine(message);
                    Console.WriteLine("************************************************************************");
                    try
                    {
                        EnviromentData enviroment = JsonConvert.DeserializeObject<EnviromentData>(message);
                        ISessionFactory _sessionEnviroment = FluentNHibernateHelper.SensorTbl_Session();
                        ISession sessionEnviroment = _sessionEnviroment.OpenSession();
                        sessionEnviroment.Clear();
                        int FarmID = sessionEnviroment.QueryOver<FarmTbl>().Where(x => x.SubprojectID == SubprojectID).Select(x => x.ID).SingleOrDefault<int>();
                        DateTime detectorTime = Utils.UnixTimeStampToDateTime(enviroment.detectorTime.ToString());
                        EnvTHITbl EnvObj = new EnvTHITbl
                        {
                            FarmID = FarmID,
                            FreeStallId = 5,
                            TdbValue = enviroment.temperature,
                            RHValue = enviroment.humidity,
                            THIValue = enviroment.temperature - (0.55 - (0.55 * enviroment.humidity / (double)100)) * (enviroment.temperature - 58),
                            SensorLat = 35.7207,
                            SensorLng = 50.87111,
                            LastId = -1,
                            MAC = enviroment.MAC,
                            date = detectorTime
                        };
                        sessionEnviroment.Save(EnvObj);
                        sessionEnviroment.Close();
                        sessionEnviroment.Dispose();
                        _sessionEnviroment.Close();
                        _sessionEnviroment.Dispose();
                        Console.WriteLine("---------------------------------ENV---------------------------------");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ENV 105###############################################################");
                        Console.WriteLine(ex.Message);
                        Console.WriteLine("----------------------------------------------------------------------------------");
                        Console.WriteLine(ex.InnerException);
                        Console.WriteLine("######################################################################");
                        WriteLog(ex.Message, "ENV 105", message);
                        if(ex.Message.Contains("An invalid or incomplete configuration was used while creating a SessionFactory"))
                        {
                            retValue = false;
                            Process.Start("Client.exe");
                            Environment.Exit(0);
                        }
                        else
                        {
                            retValue = true;
                        }
                    }
                    break;
            }
            return retValue;
        }

        private String ParseMessage(string message, bool packetType = false)
        {
            String jsonTtype = "";
            String firstChar = message.Substring(0, 1);
            if (firstChar == "[")
            {
                jsonTtype = "NaN";
            }
            else
            {
                if(packetType)
                {
                    jsonTtype = JObject.Parse(message)["packetType"].ToString();
                    switch(jsonTtype)
                    {
                        case "2":
                            jsonTtype = "bodyTemperature";
                            break;

                        case "5":
                            jsonTtype = "ENV";
                            break;
                    }
                }
                else
                {
                    jsonTtype = JObject.Parse(message)["type"].ToString();
                }
            }
            return jsonTtype;
        }

        private String[] SplitToArray(string message)
        {
            message = (message.Substring(1) + "*").Replace("]*", "").Replace("},{", "}^{");
            String[] retValue = message.Split('^');
            return retValue;
        }

        private void WriteLog(String Message, String Value, String input)
        {
            StreamWriter writer = new StreamWriter(@"LOGS/LOG_" + DateTime.Now.ToString("yyyy-mm-dd HH-mm-ss") + "_.txt");
            writer.WriteLine("MSG: " + Message + " - (" + Value + ") - " + DateTime.Now.ToString() + "\r\n\r\n" + input);
            writer.Close();
        }

        private static void WriteLog3Hour(String input)
        {
            String FileName = "LOGS_3Hours/LOG_" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt";
            if(File.Exists(FileName))
            {
                StreamWriter writer = new StreamWriter(FileName, append: true);
                writer.WriteLine("------------------------------------------(" + DateTime.Now.ToString() + ")" + input + "\r\n\r\n");
                writer.Close();
            }
            else
            {
                StreamWriter writer = new StreamWriter(FileName);
                writer.WriteLine("------------------------------------------(" + DateTime.Now.ToString() + ")" + input + "\r\n\r\n");
                writer.Close();
            }
        }
    }
}
