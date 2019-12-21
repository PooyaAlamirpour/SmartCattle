using Elmah;
using Newtonsoft.Json;
using SmartCattle.Core;
using SmartCattle.DataAccess;
using SmartCattle.DomainClass;
using SmartCattle.Service;
using SmartCattle.Web.Helper;
using SmartCattle.Web.Push;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;

namespace SmartCattle.Web
{
    public class MyTimer
    {
        private static readonly int limit = 1000;                // IPARS data fetch limitation
        private static readonly int FetchInterval = 60000;     // 2 minutes
        private static long? lastPositionId = 0;                // last received position id
        private static long? lastActivityId = 0;                // last received activity id
        private static long? lastTemperatureId = 0;             // last received temperature id
        private static Timer Timer { get; set; }                // Timer for IPARS periodic calls 
        private static string token, apiKey, email, password;   
        static MyTimer()
        {
            #region signin / signUp IPRAS and get token
            //try
            //{
            //    IPARS ipars = new IPARS();
            //using (SmartCattleContext ctx = new SmartCattleContext())
            //{                
            //        ApplicationSetting Email = ctx.ApplicationSettings.AsQueryable().FirstOrDefault(s => s.key == "email");
            //        if (Email != null)
            //        {
            //            email = Email.value;
            //        }
            //        else
            //        {
            //            signUpResult res = ipars.signUp(new SignUpParam() { email = Setting.IPARSEmail, password = Setting.IPARSPassword,
            //                phoneNumber = Setting.IPARSPhoneNumber, projectName =Setting.IPARSProjectName }).Result;

            //            ctx.ApplicationSettings.Add(new ApplicationSetting() { key = "email", value = Setting.IPARSEmail });
            //            ctx.ApplicationSettings.Add(new ApplicationSetting() { key = "apiKey", value = res.apiKey });
            //            ctx.ApplicationSettings.Add(new ApplicationSetting() { key = "password", value = Setting.IPARSPassword });
            //            ctx.saveChanges();
            //            email = Setting.IPARSEmail;
            //        }                   

            //        ApplicationSetting Password = ctx.ApplicationSettings.AsQueryable().FirstOrDefault(s => s.key == "password");
            //        if (Password != null)
            //        {
            //            password = Password.value;
            //        }
            //        else
            //        {
            //            password = "";
            //        }                     

            //        ApplicationSetting ApiKey = ctx.ApplicationSettings.AsQueryable().FirstOrDefault(s => s.key == "apiKey");
            //        if (ApiKey != null && !string.IsNullOrEmpty(ApiKey.value))
            //        {
            //            apiKey = ApiKey.value;
            //        }
            //        else
            //        {
            //            apiKey =getApiKey(email,password);
            //            ctx.ApplicationSettings.FirstOrDefault(s => s.key == "apiKey").value = apiKey;
            //            ctx.saveChanges();
            //        }

            //        var lastActivity = ctx.ActivityStates.AsQueryable().OrderByDescending(a => a.LastRecievedId).FirstOrDefault();
            //        var lastPosition = ctx.Positions.AsQueryable().OrderByDescending(a => a.LastRecievedId).FirstOrDefault();
            //        var lastTemperature = ctx.Tempretures.AsQueryable().OrderByDescending(a => a.LastRecievedId).FirstOrDefault();


            //        if (lastActivity != null)   lastActivityId = lastActivity.LastRecievedId;
            //        if (lastPosition != null)   lastPositionId = lastPosition.LastRecievedId;
            //        if (lastTemperature != null)   lastTemperatureId = lastTemperature.LastRecievedId;

            //    }
            //SignInParam param = new SignInParam() { email = email, apiKey = apiKey, password = password };
            //signInResult signInResult = ipars.signIn(param).Result;
            //    if (signInResult != null)
            //    {
            //        token = signInResult.token;
            //    }
              
            //}
            //catch (Exception exc)
            //{
            //    ErrorSignal.FromCurrentContext().Raise(exc);
            //}
            #endregion
        }
        public static void StartTimer()
        {
            //Timer = new System.Timers.Timer(FetchInterval);
            //Timer.Elapsed += SendTimerElapsed; 
            //Timer.Start();
        }
        private static void SendTimerElapsed(object sender, ElapsedEventArgs e)
        {
            //Timer.Stop();
            try
            {
                //getActivityData();
                //getPositionData();
                //getTemperatureData(); 
                //PushHub push = new PushHub();
                //push.SendNotification(); 
                //Rule rule = new Rule();
                //rule.RunRules();         
            }
            catch (Exception exp)
            {
                //ErrorSignal.FromCurrentContext().Raise(exp); 
            }
            finally
            {
                //Timer.Start();
            }
        }
        public static BehaviorState MapToActivity(string value)
        {
            switch (value)
            {
                case "sitting": return BehaviorState.sitting;
                case "eating": return BehaviorState.eating;
                case "drinking": return BehaviorState.drinking;  
                case "rumination": return BehaviorState.rumination;
                case "standing": return BehaviorState.standing;
                case "walking": return BehaviorState.walking;
                default: return BehaviorState.nonActive;
            }
        } // map string activity to Enum
       // public static void getPositionData()
       // {
       //     try
       //     {
       //         HttpClient client = new HttpClient();
       //         client.Timeout = new TimeSpan(0, 1, 0);
       //         string content = "";
       //         List<Position> Positions = new List<Position>();
       //         HttpResponseMessage res = new HttpResponseMessage();
       //         string url = Setting.GetPositionUrl + "?tokenId=" + token + "&positionId=" + lastPositionId + "&limit="+limit;
       //         Task.Run(async () => { res = await client.GetAsync(url); }).Wait();
       //         if (res.IsSuccessStatusCode)
       //         {
       //             Task.Run(async () => { content = await res.Content.ReadAsStringAsync(); }).Wait();
       //             Positions = JsonConvert.DeserializeObject<List<Position>>(content);
       //             using (SmartCattleContext context = new SmartCattleContext())
       //             {
       //                 context.Configuration.LazyLoadingEnabled = true;
       //                 foreach (var packet in Positions)
       //                 {
       //                     if (packet.id > lastPositionId)
       //                     {
       //                         lastPositionId = packet.id;
       //                     }
       //                     Sensor sensor = context.Sensors.AsQueryable().Where(s => s.MacAddress == packet.MAC).FirstOrDefault();
       //                     if (sensor != null)
       //                     {
       //                         context.Positions.Add(new CattlePosition()
       //                         {
       //                             date = DateHelper.TimeStampToDateTime(packet.detectorTime),
       //                             Latitude=packet.Latitude,
       //                             Longitude=packet.Longitude,
       //                             LatLong = Utility.CreatePoint(packet.Latitude,packet.Longitude),
       //                             FarmID = sensor.FarmID,
       //                             cattleId = (int)sensor.cattleId,
       //                             LastRecievedId = packet.id
       //                         });                            
       //                     }
       //                 }
       //                 context.saveChanges();
       //             }
       //         }
       //     }
       //     catch(Exception exc)
       //     {
       //         ErrorSignal.FromCurrentContext().Raise(exc);
       //     }
       //} // get position data and save in Database
       // public static void getTemperatureData()
       // {
       //     try
       //     {
       //         HttpClient client = new HttpClient();
       //         client.Timeout = new TimeSpan(0, 1, 0);
       //         string content = "";
       //         List<BodyTemperature> Positions = new List<BodyTemperature>();
       //         HttpResponseMessage res = new HttpResponseMessage();
       //         string url = Setting.GetTemperatureUrl + "?tokenId=" + token + "&temperatureId=" + lastTemperatureId + "&limit="+limit;
       //         Task.Run(async () => { res = await client.GetAsync(url); }).Wait();
       //         if (res.IsSuccessStatusCode)
       //         {
       //             Task.Run(async () => { content = await res.Content.ReadAsStringAsync(); }).Wait();
       //             Positions = JsonConvert.DeserializeObject<List<BodyTemperature>>(content);

       //             using (SmartCattleContext context = new SmartCattleContext())
       //             {
       //                 context.Configuration.LazyLoadingEnabled = true;
       //                 foreach (var packet in Positions)
       //                 {
       //                     if (packet.id > lastTemperatureId)
       //                     {
       //                         lastTemperatureId = packet.id;
       //                     }
       //                     Sensor sensor = context.Sensors.AsQueryable().Where(s => s.MacAddress == packet.MAC).FirstOrDefault();
       //                     if (sensor != null)
       //                     {
       //                         context.Tempretures.Add(new CattleTempreture()
       //                         {
       //                             cattleId = (int)sensor.cattleId,
       //                             date = DateHelper.TimeStampToDateTime(packet.detectorTime),
       //                             FarmID = sensor.FarmID,
       //                             value = packet.bodyTemperature,
       //                             LastRecievedId = packet.id
       //                         });
       //                     }
       //                 }
       //                 context.saveChanges();
       //             }
       //         }
       //     }
       //     catch(Exception exc)
       //     {
       //         ErrorSignal.FromCurrentContext().Raise(exc);
       //     }
       // } // get temperature data and save in database
       // public static void getActivityData()
       // {
       //     try
       //     {
       //         HttpClient client = new HttpClient() { Timeout= TimeSpan.FromMilliseconds(5000) };
       //         client.Timeout = new TimeSpan(0, 1, 0);
       //         string content = "";
       //         decimal total = 0;
       //         List<CowActivity> Activities = new List<CowActivity>();
       //         HttpResponseMessage res = new HttpResponseMessage();
       //         string url = Setting.GetActivityUrl + "?tokenId=" + token + "&activityId=" + lastActivityId + "&limit="+limit;
       //         Task.Run(async () => { res = await client.GetAsync(url); }).Wait();
       //         if (res.IsSuccessStatusCode)
       //         {
       //             Task.Run(async () => { content = await res.Content.ReadAsStringAsync(); }).Wait();
       //             Activities = JsonConvert.DeserializeObject<List<CowActivity>>(content);

       //             using (SmartCattleContext context = new SmartCattleContext())
       //             {
       //                 var sensors = context.Sensors.AsEnumerable();
       //                 List<CattleActivityState> activityList = new List<CattleActivityState>();

       //                 context.Configuration.LazyLoadingEnabled = true;

       //                 foreach (var packet in Activities)
       //                 {
       //                     total = packet.activity.drinking + packet.activity.eating + packet.activity.ruminating + packet.activity.sitting +
       //                         packet.activity.standing + packet.activity.walking;

       //                     if (packet.id > lastActivityId)
       //                     {
       //                         lastActivityId = packet.id;
       //                     }
       //                     Sensor sensor = sensors.Where(s => s.MacAddress == packet.MAC).FirstOrDefault();
       //                     if (sensor != null && total!=0)
       //                     {
       //                         activityList.Add(new CattleActivityState()
       //                         {
       //                             cattleId = (int)(sensor.cattleId),
       //                             Sitting = packet.activity.sitting/total*100,
       //                             Standing = packet.activity.standing / total * 100,
       //                             Walking = packet.activity.walking / total * 100,
       //                             Eating = packet.activity.eating / total * 100,
       //                             Rumination = packet.activity.ruminating / total * 100,
       //                             Drinking = packet.activity.drinking / total * 100,
       //                             date = DateHelper.TimeStampToDateTime(packet.detectorTime),
       //                             jsonedActivities = packet.activity.ToString(),
       //                             FarmID = sensor.FarmID,
       //                             LastRecievedId = packet.id,
       //                         });                              
       //                     }
       //                 }
       //             context.ActivityStates.AddRange(activityList);
       //             context.saveChanges();
       //             }
       //         }
       //     }
       //     catch(Exception exc)
       //     {
       //         ErrorSignal.FromCurrentContext().Raise(exc);
       //     }
       // } // get activity data and save in database
       // public static string getApiKey(string email ,string password)
       // {
       //     try
       //     {
       //         HttpClient client = new HttpClient();
       //         client.Timeout = new TimeSpan(0, 1, 0);
       //         string content = "";
       //         string ApiKey="";
       //         HttpResponseMessage res = new HttpResponseMessage();
       //         string url = Setting.GetApiKeyUrl + "?email=" + email + "&password=" + password;
       //         Task.Run(async () => { res = await client.GetAsync(url); }).Wait();
       //         if (res.IsSuccessStatusCode)
       //         {
       //             Task.Run(async () => { content = await res.Content.ReadAsStringAsync(); }).Wait();
       //             apiKey = JsonConvert.DeserializeObject<ApiKey>(content).apiKey;                    
       //             return apiKey;
       //         }
       //         return "";
       //     }
       //     catch
       //     {
       //         return "";
       //     }
       // } // get apikey
    } 
    public class Position
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string MAC { get; set; }
        public string detectorTime { get; set; }
        public int id { get; set; }
    } // position DTO
    public class BodyTemperature
    {
        public double bodyTemperature { get; set; }
        public string MAC { get; set; }
        public string detectorTime { get; set; }
        public int id { get; set; }
    } // body temperature DTO
    public class CowActivity
    {
        public Activity activity { get; set; }
        public string MAC { get; set; }
        public string detectorTime { get; set; }
        public int id { get; set; }
    } // Activity DTO
    public class ApiKey
    {
        public int response { get; set; }
        public string message { get; set; }
        public string apiKey { get; set; }
    } // Api key POCO
    public class Activity
    { 
        public int standing { get; set; }
        public int walking { get; set; }
        public int eating { get; set; }
        public int drinking { get; set; }
        public int sitting { get; set; }
        public int ruminating { get; set; }
        public override string ToString()
        {
            return "{\"standing\":" + standing + ",\"walking\":" + walking + ",\"eating\":" + eating + ",\"drinking\":" + drinking + ",\"sitting\":" + sitting + ",\"ruminating\":" + ruminating+"}";
        }
    } // Activity POCO
}
