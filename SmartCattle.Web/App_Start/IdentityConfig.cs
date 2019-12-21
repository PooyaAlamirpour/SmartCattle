using System;
using System.Security.Claims;
using System.Threading.Tasks;
using SmartCattle.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using SmartCattle.DomainClass;
using System.Net.Mail;
using System.Net;

namespace SmartCattle.Web
{
    
    public class EmailService : IIdentityMessageService
    {       
        public Task SendAsync(IdentityMessage message)
        { 
          return configSMTPasync(message);
          
           // return Task.FromResult(0);
        }

        private async Task configSMTPasync(IdentityMessage message)
        { 
            string emailFrom = "support@smartcattle.ir";
            string password = "HOse!n1234";

            MailAddress ma_from = new MailAddress(emailFrom, "smart cattle");
            MailAddress ma_to = new MailAddress(message.Destination, message.Destination);
            string s_password = password;
            string s_subject = message.Subject;
            string s_body =message.Body;

            SmtpClient smtp = new SmtpClient
            {
                Host = "mail.smartcattle.ir",
                //change the port to prt 587. This seems to be the standard for Google smtp transmissions.
                Port = 587,
                //enable SSL to be true, otherwise it will get kicked back by the Google server.
                EnableSsl = false,
                Timeout=10000,
                //The following properties need set as well
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = true,
                Credentials = new NetworkCredential(ma_from.Address, s_password)
            };


            using (MailMessage mail = new MailMessage(ma_from, ma_to)
            {
                Subject = s_subject,
                Body = s_body

            })

                try
                {
                    
                    smtp.Send(mail);
                  
                }
                catch (Exception ex)
                {
                    
                }

            return;

        }

    } 

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class SmartCattleUserManager : UserManager<SmartCattleUser>
    {
        public SmartCattleUserManager(IUserStore<SmartCattleUser> store)
            : base(store)
        {
        }

        public static SmartCattleUserManager Create(IdentityFactoryOptions<SmartCattleUserManager> options, IOwinContext context) 
        {
            var manager = new SmartCattleUserManager(new UserStore<SmartCattleUser>(new DataAccess.SmartCattleContext()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<SmartCattleUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 8,
                RequireNonLetterOrDigit = true, 
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<SmartCattleUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<SmartCattleUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<SmartCattleUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    public class SmartCattleRoleManager : RoleManager<UserRole>
    {
        public SmartCattleRoleManager(IRoleStore<UserRole, string> store) : base(store)
        {
        }
        public static SmartCattleRoleManager Create(IdentityFactoryOptions<SmartCattleRoleManager> options,
            IOwinContext context)
        {
            return new SmartCattleRoleManager(
            new RoleStore<UserRole>(new DataAccess.SmartCattleContext()));
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<SmartCattleUser, string>
    {
        public ApplicationSignInManager(SmartCattleUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override  Task<ClaimsIdentity> CreateUserIdentityAsync(SmartCattleUser user)
        {            
            return user.GenerateUserIdentityAsync((SmartCattleUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<SmartCattleUserManager>(), context.Authentication);
        }

        //public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<SmartCattleUser> manager)
        //{
        //    // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
        //    var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
        //    // Add custom user claims here
        //    return userIdentity;
        //}
    }
}
