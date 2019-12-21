using SmartCattle.Core;
using SmartCattle.DataAccess;
using SmartCattle.DomainClass;
using SmartCattle.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace SmartCattle.Web
{
   public class IPARS
    {
        BaseServices<ApplicationSetting> settingService;
        public async Task<signUpResult> signUp(SignUpParam param)
        {
            try
            {
                settingService = new BaseServices<ApplicationSetting>(new DataAccess.GenericUnitOfWork<ApplicationSetting>(new SmartCattleContext()));

                string json = new JavaScriptSerializer().Serialize(param);
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("param", json));

                using (var httpClient = new HttpClient())
                {
                    using (var content = new FormUrlEncodedContent(parameters))
                    {
                        content.Headers.Clear();
                        content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                        HttpResponseMessage response = await httpClient.PostAsync(Setting.SignUpUrl, content);

                        string res = await response.Content.ReadAsStringAsync();
                        return new JavaScriptSerializer().Deserialize<signUpResult>(res);
                    }
                }
            }
            catch (Exception ex)
            {
                return new signUpResult() { message = "exception", response = "null", apiKey = "null" };
            }
        }
        public async Task<signInResult> signIn(SignInParam param)
        {
            try
            {
                settingService = new BaseServices<ApplicationSetting>(new DataAccess.GenericUnitOfWork<ApplicationSetting>(new SmartCattleContext()));

                string json = new JavaScriptSerializer().Serialize(param);
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("param", json));

                using (var httpClient = new HttpClient())
                {
                    using (var content = new FormUrlEncodedContent(parameters))
                    {
                        content.Headers.Clear();
                        content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                        HttpResponseMessage response = await httpClient.PostAsync(Setting.SignInUrl, content);

                        string res = await response.Content.ReadAsStringAsync();
                        return new JavaScriptSerializer().Deserialize<signInResult>(res);
                    }
                }
            }
            catch(Exception ex)
            {
                return new signInResult() { message = "exception", response = "null", token = "null" };
            }
        }
     

    }

    
    public class SignUpParam
    {
        public string projectName { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string password { get; set; }
    }
    public class SignInParam
    {
        public string email { get; set; }
        public string apiKey { get; set; }
        public string password { get; set; }
    }
    public class signUpResult
    {
        public string response { get; set; }
        public string message { get; set; }
        public string apiKey { get; set; }
    }
    public class signInResult
    {
        public string response { get; set; }
        public string message { get; set; }
        public string token { get; set; }
    }
    public class GetFarmMapParam
    {
        public string tokenId { get; set; }
        public int subProjectId { get; set; } 
    }


}
