using SmartCattle.DataAccess;
using SmartCattle.DomainClass;
using SmartCattle.Web.CustomFilters;
using System.Web.Mvc;
using System.Web;
using System.IO;
using System.Web.Routing;
using SmartCattle.Service;
using Microsoft.AspNet.Identity;
using SmartCattle.Core;
using SmartCattle.Web.Helper;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.Threading;
using System.Collections.Generic;
using System.Data.Entity;
using Microsoft.AspNet.SignalR;
using System.Collections;
using SmartCattle.Web.Domain;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using NHibernate;

namespace SmartCattle.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Blank()
        {
            return View();
        }

        public ActionResult Alerts()
        {
            return View();
        }

        [HttpPost]
        public String SitePath(String path)
        {
            String[] SplitedPath = path.Split('_');
            String retValue = "";
            for (int i = 0; i < SplitedPath.Length - 1; i++)
            {
                retValue += Localization.getString(SplitedPath[i]) + " > ";
            }
            retValue += Localization.getString(SplitedPath[SplitedPath.Length - 1]);
            retValue = "<a href=\"/Home/Index\">" + Localization.getString("Dashboard") + "</a>" + " > " + retValue;
            retValue = retValue.Replace("> #", "");
            return retValue;
        }
        
        [Localization("fa")]
        public async Task<ActionResult> Index()
        {
            HomePageViewModel HPVM = new Controllers.HomePageViewModel();
            DateTime pastSevenDays = DateTime.Now.AddDays(-7);
            return View(HPVM);
        }

        public ActionResult changeLanguage(string langCulture)
        {
            String[] splitedlangCulture = langCulture.Split('-');
            string language = splitedlangCulture[0];
            string culture = splitedlangCulture[1];
            LocalizationAttribute.Current_Language_Culture = langCulture;

            var fullUrl = Request.UrlReferrer.ToString();
            var questionMarkIndex = fullUrl.IndexOf('?');
            string queryString = null;
            string url = fullUrl;
            if (questionMarkIndex != -1) // There is a QueryString
            {
                url = fullUrl.Substring(0, questionMarkIndex);
                queryString = fullUrl.Substring(questionMarkIndex + 1);
            }

            // Arranges
            var request = new HttpRequest(null, url, queryString);
            var response = new HttpResponse(new StringWriter());
            var httpContext = new HttpContext(request, response);

            var routeData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));

            // Extract the data    
            var values = routeData.Values;
            var controllerName = values["controller"];
            var actionName = values["action"];
            var areaName = values["area"];
            
            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(string.Format("{0}-{1}", language, culture));
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(string.Format("{0}-{1}", language, culture));
            }
            catch (CultureNotFoundException ex)
            {
                // TODO
            }
            catch (ArgumentNullException ex)
            {
                //TODO
            }
            Helper.Helper.setCurrentCulture(langCulture);
            return Redirect("/" + langCulture + "/" + controllerName + "/" + actionName);
        }

        [HttpPost]
        public ContentResult Weather()
        {
            string html;
            //ISession mContext = Context.Open();
            //for (int i = 1435; i < 10000; i++)
            //{
            //    using (var client = new WebClient())
            //    {
            //        String num = "";
            //        if(i <= 9)
            //        {
            //            num = "0" + i.ToString();
            //        }
            //        else
            //        {
            //            num = i.ToString();
            //        }
            //        try
            //        {
            //            html = client.DownloadString("https://weather.com/weather/hourbyhour/l/IRXX" + num + ":1:IR");
            //            int indexOfTitle_start = html.IndexOf("<title data-react-helmet>");
            //            String CrystallyClear_indexOfTitle = html.Substring(indexOfTitle_start);

            //            int indexOfTitle_end = CrystallyClear_indexOfTitle.IndexOf("Weather.com</title>");
            //            CrystallyClear_indexOfTitle = CrystallyClear_indexOfTitle.Substring(0, indexOfTitle_end);

            //            CrystallyClear_indexOfTitle = CrystallyClear_indexOfTitle.Replace("<title data-react-helmet>", "").Replace(", Iran - The Weather Channel | ", "").Replace("Hourly Weather Forecast for ", "");

            //            WeatherTbl _Weather = new WeatherTbl()
            //            {
            //                WeatherCode = "IRXX" + num + ":1:IR",
            //                Name = CrystallyClear_indexOfTitle
            //            };
            //            mContext.Save(_Weather);
            //        }
            //        catch (Exception)
            //        {
            //            WeatherTbl _Weather = new WeatherTbl()
            //            {
            //                WeatherCode = "NaN",
            //                Name = "NaN"
            //            };
            //            mContext.Save(_Weather);
            //        }

            //        Thread.Sleep(3000);
            //    }
            //}
            //Context.Close(mContext);
            String USerLocation = Helper.Helper.getUserLocation();
            using (var client = new WebClient())
            {
                //if(Helper.Helper.getCurrentCulture() == Helper.Helper.FA)
                {
                    ISession mContext = Context.Open();
                    WeatherTbl _Weather = mContext.QueryOver<WeatherTbl>().Where(x => x.Name == USerLocation).Take(1).SingleOrDefault();
                    if(_Weather != null)
                    {
                        html = client.DownloadString("https://weather.com/weather/hourbyhour/l/" + _Weather.WeatherCode);
                    }
                    else
                    {
                        html = client.DownloadString("https://weather.com/weather/hourbyhour/l/IRXX0018:1:IR");
                    }
                    Context.Close(mContext);
                }
                //else
                //{
                //    //html = client.DownloadString("https://weather.com/weather/hourbyhour/l/USCA0638:1:US");
                //    html = client.DownloadString("https://weather.com/weather/hourbyhour/l/USCA0638:1:US");
                //}
            }
            int indexOfOpenedTbody = html.IndexOf("<table");
            String CrystallyClear = html.Substring(indexOfOpenedTbody);
            int indexOfClosedTbody = CrystallyClear.IndexOf("</table>");
            CrystallyClear = CrystallyClear.Substring(0, indexOfClosedTbody);
            int indexOfOpenedIcon = CrystallyClear.IndexOf("<icon");
            int indexOfClosedIcon = CrystallyClear.IndexOf("</icon>");
            String tmpSaveString = CrystallyClear.Substring(indexOfOpenedIcon, (indexOfClosedIcon - indexOfOpenedIcon) + 7);
            CrystallyClear = CrystallyClear.Replace(tmpSaveString, "");
            int indexOfOpenedthead = CrystallyClear.IndexOf("<thead");
            int indexOfClosedthead = CrystallyClear.IndexOf("</thead>");
            tmpSaveString = CrystallyClear.Substring(indexOfOpenedthead, (indexOfClosedthead - indexOfOpenedthead) + 8);
            CrystallyClear = CrystallyClear.Replace(tmpSaveString, "");
            List<WeatherData> _WeatherData = new List<WeatherData>();
            while (CrystallyClear.Length > 65)
            {
                WeatherData item = new WeatherData();
                int indexOfOpenedtr = CrystallyClear.IndexOf("<tr");
                int indexOfClosedtr = CrystallyClear.IndexOf("</tr>");
                tmpSaveString = CrystallyClear.Substring(indexOfOpenedtr, (indexOfClosedtr - indexOfOpenedtr) + 5);
                int indexOfOpenedtd = tmpSaveString.IndexOf("<td");
                int indexOfClosedtd = tmpSaveString.IndexOf("</td>");
                var regex = new Regex(Regex.Escape(tmpSaveString.Substring(indexOfOpenedtd, (indexOfClosedtd - indexOfOpenedtd) + 5)));
                var newText = regex.Replace(CrystallyClear, "", 1);
                CrystallyClear = newText;// CrystallyClear.Replace(tmpSaveString.Substring(indexOfOpenedtd, (indexOfClosedtd - indexOfOpenedtd) + 5), "");

                indexOfOpenedtd = CrystallyClear.IndexOf("<td");
                indexOfClosedtd = CrystallyClear.IndexOf("</td>");
                item.Date = CrystallyClear.Substring(indexOfOpenedtd, (indexOfClosedtd - indexOfOpenedtd) + 5);
                regex = new Regex(Regex.Escape(item.Date));
                newText = regex.Replace(CrystallyClear, "", 1);
                CrystallyClear = newText;// CrystallyClear.Replace(item.Date, "");

                indexOfOpenedtd = CrystallyClear.IndexOf("<td");
                indexOfClosedtd = CrystallyClear.IndexOf("</td>");
                item.Status = CrystallyClear.Substring(indexOfOpenedtd, (indexOfClosedtd - indexOfOpenedtd) + 5);
                regex = new Regex(Regex.Escape(item.Status));
                newText = regex.Replace(CrystallyClear, "", 1);
                CrystallyClear = newText;// CrystallyClear.Replace(item.Status, "");

                indexOfOpenedtd = CrystallyClear.IndexOf("<td");
                indexOfClosedtd = CrystallyClear.IndexOf("</td>");
                item.Temp = CrystallyClear.Substring(indexOfOpenedtd, (indexOfClosedtd - indexOfOpenedtd) + 5);
                regex = new Regex(Regex.Escape(item.Temp));
                newText = regex.Replace(CrystallyClear, "", 1);
                CrystallyClear = newText;// CrystallyClear.Replace(item.Temp, "");

                indexOfOpenedtd = CrystallyClear.IndexOf("<td");
                indexOfClosedtd = CrystallyClear.IndexOf("</td>");
                item.Feel = CrystallyClear.Substring(indexOfOpenedtd, (indexOfClosedtd - indexOfOpenedtd) + 5);
                regex = new Regex(Regex.Escape(item.Feel));
                newText = regex.Replace(CrystallyClear, "", 1);
                CrystallyClear = newText;// CrystallyClear.Replace(item.Feel, "");

                indexOfOpenedtd = CrystallyClear.IndexOf("<td");
                indexOfClosedtd = CrystallyClear.IndexOf("</td>");
                item.Precip = CrystallyClear.Substring(indexOfOpenedtd, (indexOfClosedtd - indexOfOpenedtd) + 5);
                regex = new Regex(Regex.Escape(item.Precip));
                newText = regex.Replace(CrystallyClear, "", 1);
                CrystallyClear = newText;// CrystallyClear.Replace(item.Precip, "");

                indexOfOpenedtd = CrystallyClear.IndexOf("<td");
                indexOfClosedtd = CrystallyClear.IndexOf("</td>");
                item.Humidity = CrystallyClear.Substring(indexOfOpenedtd, (indexOfClosedtd - indexOfOpenedtd) + 5);
                regex = new Regex(Regex.Escape(item.Humidity));
                newText = regex.Replace(CrystallyClear, "", 1);
                CrystallyClear = newText;// CrystallyClear.Replace(item.Humidity, "");

                indexOfOpenedtd = CrystallyClear.IndexOf("<td");
                indexOfClosedtd = CrystallyClear.IndexOf("</td>");
                item.Wind = CrystallyClear.Substring(indexOfOpenedtd, (indexOfClosedtd - indexOfOpenedtd) + 5);
                regex = new Regex(Regex.Escape(item.Wind));
                newText = regex.Replace(CrystallyClear, "", 1);
                CrystallyClear = newText;// CrystallyClear.Replace(item.Wind, "");

                indexOfOpenedtr = CrystallyClear.IndexOf("<tr");
                indexOfClosedtr = CrystallyClear.IndexOf("</tr>");
                tmpSaveString = CrystallyClear.Substring(indexOfOpenedtr, (indexOfClosedtr - indexOfOpenedtr) + 5);
                regex = new Regex(Regex.Escape(tmpSaveString));
                newText = regex.Replace(CrystallyClear, "", 1);
                CrystallyClear = newText;// CrystallyClear.Replace(tmpSaveString, "");

                item.Date = Regex.Replace(item.Date, "<.*?>", String.Empty);
                item.Status = Regex.Replace(item.Status, "<.*?>", String.Empty);
                String tmp1 = Regex.Replace(item.Temp, "<.*?>", String.Empty)/*.Replace("°", "").Replace("آ", "")*/;
                String tmp2 = Regex.Replace(item.Feel, "<.*?>", String.Empty)/*.Replace("°", "").Replace("آ", "")*/;
                tmp1 = tmp1.Substring(0, 2);
                tmp2 = tmp2.Substring(0, 2);

                item.Temp = (Math.Round((Convert.ToInt16(tmp1) - 32) / (double)(1.8), 0)).ToString().Replace("/", ".");
                item.Feel = (Math.Round((Convert.ToInt16(tmp2) - 32) / (double)(1.8), 0)).ToString().Replace("/", ".");
                item.Precip = Regex.Replace(item.Precip, "<.*?>", String.Empty);
                item.Humidity = Regex.Replace(item.Humidity, "<.*?>", String.Empty).Replace("%", "");
                item.Wind = Regex.Replace(item.Wind, "<.*?>", String.Empty);
                item.City = USerLocation;

                if(item.Date.Contains("pm"))
                {
                    int indexOfPm = item.Date.IndexOf("pm");
                    item.Date = item.Date.Substring(0, indexOfPm - 1);
                }
                else
                {
                    int indexOfAm = item.Date.IndexOf("am");
                    item.Date = item.Date.Substring(0, indexOfAm - 1);
                }

                _WeatherData.Add(item);
            }
            
            var result = new ContentResult
            {
                Content = JsonConvert.SerializeObject(_WeatherData),
                ContentType = "application/json"
            };

            return result;
        }

        public class WeatherData
        {
            public String Date { get; set; }
            public String Status { get; set; }
            public String Temp { get; set; }
            public String Feel { get; set; }
            public String Precip { get; set; }
            public String Humidity { get; set; }
            public String Wind { get; set; }
            public object City { get; set; }
        }

        public ActionResult Inbox()
        {
            return View();
        }

        public ActionResult Compose()
        {
            return View();
        }

        public ActionResult ViewMessage()
        {
            return View();
        }

        public ActionResult Timeline()
        {
            return View();
        }

        public ActionResult Error404()
        {
            return View();
        }

        public ActionResult Error500()
        {
            return View();
        }
    }

    public class HomePageViewModel
    {
        public IEnumerable<CattleActivityState> activities { set; get; }
        public IEnumerable<IGrouping<object,CattleTempreture>> temperatures { set; get; }
        public FreeStallTbl FreeStall { get; set; }
    }

    public class OnlyDateComparer : IEqualityComparer<DateTime>
    {
        public bool Equals(DateTime x, DateTime y)
        {
            return x.Date == y.Date;
        }

        public int GetHashCode(DateTime obj)
        {
            return 17 * base.GetHashCode();
        }
    }
}
