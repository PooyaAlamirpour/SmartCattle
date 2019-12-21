using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SmartCattle.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SmartCattle.DomainClass;
using SmartCattle.Web.Helper;
using SmartCattle.Web.CustomFilters;
using System.Collections;
using SmartCattle.Service;
using SmartCattle.DataAccess;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.Owin;
using System.Net;
using SmartCattle.Web.ExtensionMethods;
using System;
using Elmah;
using SmartCattle.Web.Areas.APIs.Models;
using System.Threading;
using NHibernate;
using SmartCattle.Web.Domain;

namespace SmartCattle.Web.Controllers
{
  
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private SmartCattleUserManager _userManager;
        private SmartCattleRoleManager _roleManager;
        BaseServices<Farm> FarmService;

        public AccountController()
        {
            FarmService = new BaseServices<Farm>(new DataAccess.GenericUnitOfWork<Farm>(new SmartCattleContext()));
        }

        public AccountController(SmartCattleUserManager userManager, ApplicationSignInManager signInManager, SmartCattleRoleManager roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager; 
        }

        public SmartCattleRoleManager RoleManager
        {
            get
            {
               return _roleManager ?? HttpContext.GetOwinContext().Get<SmartCattleRoleManager>();
            }
            set
            {
                _roleManager = value;
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public SmartCattleUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<SmartCattleUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [AllowAnonymous]
        public ActionResult Lock()
        {
            return View();
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {            
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
         
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]      
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //PRMContext mContext = new PRMContext();
            String tmpEmail = model.Email;
            String tmpPassword = model.Password;

            ISession mContext = Context.Open();
            List<UserInfo> UserInfoList = mContext.QueryOver<UserInfo>().Where(x => x.Email == tmpEmail && x.Password == tmpPassword).List().ToList();
            Context.Close(mContext);

            if (UserInfoList.Count != 0)
            {
                SmartCattleUser user = new SmartCattleUser();
                user.UserName = UserInfoList[0].Email;
                user.Id = "f397d20d-6fa7-40b4-9ef2-c0f96bc0192c";
                user.FarmID = UserInfoList[0].FarmId;
                Helper.Helper.setCurrentUser(UserInfoList[0].ID);
                Helper.Helper.setCurrentFarmId(user.FarmID);

                if(user.FarmID == -3)
                {
                    ModelState.AddModelError("", Localization.getString("Invalid login attempt. You do not have permission for login to SmartCattle Software. Pleasae contact with supoort."));
                    return View(model);
                }
                try
                {
                    var identity = UserManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    identity.AddClaim(new System.Security.Claims.Claim("username", user.UserName));
                    identity.AddClaim(new System.Security.Claims.Claim("userID", user.Id ?? ""));
                    identity.AddClaim(new System.Security.Claims.Claim("FarmID", user.FarmID.ToString() ?? ""));
                    identity.AddClaim(new System.Security.Claims.Claim("avatar", ""));
                    identity.AddClaim(new System.Security.Claims.Claim("Role", ""));

                    AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);
                }
                catch (Exception ex)
                {
                    String ack = ex.Message;
                }
                try
                {
                    var result = await SignInManager.PasswordSignInAsync("p.alamirpour@gmail.com", "h+RSA8wWh_S)", model.RememberMe, shouldLockout: false);
                    //return RedirectToLocal(returnUrl);
                    switch (result)
                    {
                        case SignInStatus.Success:
                            return RedirectToLocal(returnUrl);
                        case SignInStatus.LockedOut:
                            return View("Lockout");
                        case SignInStatus.RequiresVerification:
                            return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                        case SignInStatus.Failure:
                        default:
                            ModelState.AddModelError("", Localization.getString("Invalid login attempt."));
                            return View(model);
                    }
                }
                catch (Exception ex)
                {
                    String ack = ex.Message;
                }
            }
            else
            {
                ModelState.AddModelError("", Localization.getString("Invalid login attempt."));
            }

            return View(model);
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }             
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }
         
        // GET: /Account/Register

        [AuthenticateFilter] 
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                int farmID;
                var principal = (ClaimsPrincipal)HttpContext.User;
                if (principal.FindFirst("UserID") != null && principal.FindFirst("farmID") != null)
                {
                    int.TryParse(principal.FindFirst("farmID").Value, out farmID);
                }
                else
                {
                    AddErrors(new IdentityResult("invalid farm"));
                    return View(model);
                }

                var user = new SmartCattleUser { UserName = model.Email, Email = model.Email, FarmID = farmID };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var identity = UserManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    identity.AddClaim(new System.Security.Claims.Claim("username", user.UserName));
                    identity.AddClaim(new System.Security.Claims.Claim("userID", user.Id ?? ""));
                    identity.AddClaim(new System.Security.Claims.Claim("FarmID", user.FarmID.ToString() ?? ""));
                    identity.AddClaim(new System.Security.Claims.Claim("avatar", user.avatarUrl ?? ""));
                    identity.AddClaim(new System.Security.Claims.Claim("Role", ""));
                    AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            return View(model);
        }

        public ActionResult ManageRoles()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AssignRole(string roleName, string userId)
        {
            int FarmId;
            ClaimsPrincipal claim = (ClaimsPrincipal)HttpContext.User;
            int.TryParse(claim.Claims.FirstOrDefault(c => c.Type == "FarmID").Value, out FarmId);

            try
            {

                if (!UserManager.IsInRole(userId, roleName))
                {
                    UserRole role = RoleManager.Roles.FirstOrDefault(r => r.Name == roleName && r.FarmID == FarmId);
                    // -------------------------- remove previous roles ----------------------------------//
                    var AllRoles = RoleManager.Roles.Where(r => r.FarmID == FarmId).Select(r => r.Name).ToArray<string>();
                    foreach (var item in AllRoles)
                    {
                        await UserManager.RemoveFromRoleAsync(userId, item);
                    }
                    // -------------------------- add new role ----------------------------------//
                    await UserManager.AddToRolesAsync(userId, roleName);
                }
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }       
         
            AssignRoleModel model = new AssignRoleModel();
            model.users = UserManager.Users.Where(u => u.FarmID == FarmId).ToList();
            model.roles = RoleManager.Roles.Where(u => u.FarmID == FarmId).ToList();
            return View(model);             
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<string> DenyRole(string roleName, string userId)
        {
            try
            {
                int FarmId;
                ClaimsPrincipal claim = (ClaimsPrincipal)HttpContext.User;
                int.TryParse(claim.Claims.FirstOrDefault(c => c.Type == "FarmID").Value, out FarmId);

                if (UserManager.IsInRole(userId, roleName))
                {
                    await UserManager.RemoveFromRoleAsync(userId, roleName);
                }
                UserRole role = RoleManager.Roles.FirstOrDefault(r => r.Name == roleName && r.FarmID == FarmId);
            }
            catch(Exception ex)
            {

            }
             
            return "ok";
        }

        [HttpGet]
        [CheckRole]
        public ActionResult AssignRole()
        {
            int FarmId;
            ClaimsPrincipal claim = (ClaimsPrincipal)HttpContext.User;
            var farmClaim = claim.Claims.FirstOrDefault(c => c.Type == "FarmID");
            string _FarmId = farmClaim != null ? farmClaim.Value : "-1";
            int.TryParse(_FarmId, out FarmId);
            AssignRoleModel model = new AssignRoleModel();
            model.users = UserManager.Users.Where(u => u.FarmID == FarmId).ToList();
            model.roles = RoleManager.Roles.Where(u => u.FarmID == FarmId).ToList();
            return View(model);
        }

        public class SystemRole_FarmView
        {
            public virtual int ID { get; set; }
            public virtual string jName { get; set; }
            public virtual string uId { get; set; }
            public virtual string Permissions { get; set; }
            public virtual string Comment { get; set; }
            public virtual string FarmName { get; set; }
        }

        public ActionResult CreateRole()
        {
            ViewModel_Role VM = new ViewModel_Role();
            List<RolesList_UserTbl> RoleList = new List<RolesList_UserTbl>();
            List<FarmTbl> FarmList = new List<FarmTbl>();
            List<RolesList_FarmTbl> SystemRole_Farm = new List<RolesList_FarmTbl>();
            List<RolesList_StaffTbl> SystemRole_Staff = new List<RolesList_StaffTbl>();
            List<RolesList_UserTbl> AccessRoleList = new List<RolesList_UserTbl>();
            List<SystemRole_FarmView> AccessSystemRole_Farm = new List<SystemRole_FarmView>();
            List<RolesList_StaffTbl> AccessSystemRole_Staff = new List<RolesList_StaffTbl>();

            int CurrentFarmID = Helper.Helper.getCurrentFarmId();
            int userID = Helper.Helper.getCurrentUserId();
            ISession mContext = Context.Open();
            List<UserInfo> user_permissions = mContext.QueryOver<UserInfo>().Where(x => x.ID == userID).List().ToList();
            if (CurrentFarmID != 0)
            {
                RoleList = mContext.QueryOver<RolesList_UserTbl>().Where(x => x.FarmId == CurrentFarmID).List().ToList();
                SystemRole_Farm = mContext.QueryOver<RolesList_FarmTbl>().List().ToList();
                SystemRole_Staff = mContext.QueryOver<RolesList_StaffTbl>().List().ToList();
                FarmList = mContext.QueryOver<FarmTbl>().List().ToList();
                List<ActionControllerListTbl> PermissionsList = mContext.QueryOver<ActionControllerListTbl>().List().ToList();

                for (int i = 0; i < RoleList.Count; i++)
                {
                    String str = RoleList[i].Permissions;
                    for (int n = 0; n < PermissionsList.Count; n++)
                    {
                        str = str.Replace(PermissionsList[n].Controller + "-" + PermissionsList[n].Action,
                            PermissionsList[n].Controller + "-" + PermissionsList[n].Action + "-" + PermissionsList[n].Comment);
                    }
                    RoleList[i].Permissions = str;
                }
                for (int i = 0; i < SystemRole_Farm.Count; i++)
                {
                    String str = SystemRole_Farm[i].Permissions;
                    for (int n = 0; n < PermissionsList.Count; n++)
                    {
                        str = str.Replace(PermissionsList[n].Controller + "-" + PermissionsList[n].Action,
                            PermissionsList[n].Controller + "-" + PermissionsList[n].Action + "-" + PermissionsList[n].Comment);
                    }
                    SystemRole_Farm[i].Permissions = str;
                }
                for (int i = 0; i < SystemRole_Staff.Count; i++)
                {
                    String str = SystemRole_Staff[i].Permissions;
                    for (int n = 0; n < PermissionsList.Count; n++)
                    {
                        str = str.Replace(PermissionsList[n].Controller + "-" + PermissionsList[n].Action,
                            PermissionsList[n].Controller + "-" + PermissionsList[n].Action + "-" + PermissionsList[n].Comment);
                    }
                    SystemRole_Staff[i].Permissions = str;
                }

                //////////////////////////////////////////////////////////
                Boolean f_find = true;

                int CurrentUserID = Helper.Helper.getCurrentUserId();
                if(CurrentUserID != 0)
                {
                    String CurrentUserRole = Helper.Helper.getCurrentRoleuId();
                    String CurrentUserPermissionList = "";
                    if (Helper.Helper.getCurrentFarmId() == -1)
                    {
                        CurrentUserPermissionList = mContext.QueryOver<RolesList_StaffTbl>().Where(x => x.uId == CurrentUserRole).Select(x => x.Permissions).SingleOrDefault<String>();
                    }
                    else
                    {
                        CurrentUserPermissionList = mContext.QueryOver<RolesList_UserTbl>().Where(x => x.uId == CurrentUserRole).Select(x => x.Permissions).SingleOrDefault<String>();
                    }

                    String[] SplitedCurrentUserPermissionList = null;
                    if (CurrentUserPermissionList == null)
                    {
                        UserInfo _user = mContext.QueryOver<UserInfo>().Where(x => x.ID == CurrentUserID).SingleOrDefault<UserInfo>();
                        if(_user != null)
                        {
                            if (_user.FarmId == -1)
                            {
                                CurrentUserPermissionList = mContext.QueryOver<RolesList_StaffTbl>().Where(x => x.uId == CurrentUserRole).Select(x => x.Permissions).SingleOrDefault<String>();
                            }
                            else
                            {
                                CurrentUserPermissionList = mContext.QueryOver<RolesList_UserTbl>().Where(x => x.uId == CurrentUserRole).Select(x => x.Permissions).SingleOrDefault<String>();
                            }
                            SplitedCurrentUserPermissionList = CurrentUserPermissionList.Split(',');
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        SplitedCurrentUserPermissionList = CurrentUserPermissionList.Split(',');
                    }

                    for (int i = 0; i < RoleList.Count; i++)
                    {
                        String tmpPermission = mContext.QueryOver<RolesList_UserTbl>().Where(x => x.uId == RoleList[i].uId).Select(x => x.Permissions).SingleOrDefault<String>();
                        if (tmpPermission != null)
                        {
                            String[] SplitedtmpPermission = tmpPermission.Split(',');

                            if (SplitedCurrentUserPermissionList.Length >= SplitedtmpPermission.Length)
                            {
                                foreach (var ActionController in SplitedtmpPermission)
                                {
                                    if (!Helper.Helper.Find(ActionController, SplitedCurrentUserPermissionList))
                                    {
                                        f_find = false;
                                    }
                                }
                                if (f_find)
                                {
                                    AccessRoleList.Add(RoleList[i]);
                                }
                            }
                        }
                        else
                        {

                        }
                    }
                    /********************************************************/
                    f_find = true;
                    for (int i = 0; i < SystemRole_Farm.Count; i++)
                    {
                        String tmpPermission = mContext.QueryOver<RolesList_FarmTbl>().Where(x => x.uId == SystemRole_Farm[i].uId).Select(x => x.Permissions).SingleOrDefault<String>();
                        if (tmpPermission != null)
                        {
                            String[] SplitedtmpPermission = tmpPermission.Split(',');
                            if (SplitedCurrentUserPermissionList.Length >= SplitedtmpPermission.Length)
                            {
                                foreach (var ActionController in SplitedtmpPermission)
                                {
                                    if (!Helper.Helper.Find(ActionController.Replace("\r\n", ""), SplitedCurrentUserPermissionList))
                                    {
                                        f_find = false;
                                    }
                                }
                                if (f_find)
                                {
                                    List<FarmTbl> FarmListUsed = mContext.QueryOver<FarmTbl>().Where(x => x.FarmTypeUId == SystemRole_Farm[i].uId).List().ToList();
                                    String FarmListStr = "";
                                    foreach (var item in FarmListUsed)
                                    {
                                        FarmListStr += item.FarmName + ", ";
                                    }
                                    if(FarmListStr.Length != 0)
                                    {
                                        FarmListStr = FarmListStr.Remove(FarmListStr.Length - 2);
                                    }
                                    SystemRole_FarmView tmpFarm = new SystemRole_FarmView()
                                    {
                                        ID = SystemRole_Farm[i].ID,
                                        Comment = SystemRole_Farm[i].Comment,
                                        FarmName = FarmListStr,
                                        jName = SystemRole_Farm[i].jName,
                                        Permissions = SystemRole_Farm[i].Permissions,
                                        uId = SystemRole_Farm[i].uId
                                    };
                                    AccessSystemRole_Farm.Add(tmpFarm);
                                }
                            }
                        }
                        else
                        {

                        }
                    }
                    /********************************************************/
                    f_find = true;
                    for (int i = 0; i < SystemRole_Staff.Count; i++)
                    {
                        String tmpPermission = mContext.QueryOver<RolesList_StaffTbl>().Where(x => x.uId == SystemRole_Staff[i].uId).Select(x => x.Permissions).SingleOrDefault<String>();
                        if (tmpPermission != null)
                        {
                            String[] SplitedtmpPermission = tmpPermission.Split(',');

                            if (SplitedCurrentUserPermissionList.Length >= SplitedtmpPermission.Length)
                            {
                                foreach (var ActionController in SplitedtmpPermission)
                                {
                                    if (!Helper.Helper.Find(ActionController, SplitedCurrentUserPermissionList))
                                    {
                                        f_find = false;
                                    }
                                }
                                if (f_find)
                                {
                                    AccessSystemRole_Staff.Add(SystemRole_Staff[i]);
                                }
                            }
                        }
                        else
                        {

                        }
                    }
                    //////////////////////////////////////////////////////////
                }
                else
                {

                }
                
            }
            Context.Close(mContext);

            VM.RoleList = AccessRoleList;
            VM.SystemRole_Farm = AccessSystemRole_Farm;
            VM.SystemRole_Staff = AccessSystemRole_Staff;
            VM.FarmList = FarmList;

            return View(VM);
        }

        [AuthenticateFilter]
        public String SaveRole(String RoleName, String RoleComment, String PermissionList, String RoleType, String FarmId, String Controller_Action_List)
        {
            String[] splitedPermissionList = PermissionList.Replace("onoffswitch", "").Split('-');
            PermissionList = PermissionList.Replace("onoffswitch_", "");

            String[] splitedController_Action_List = Controller_Action_List.Split('_');

            String Permissions = PermissionList;

            Helper.Helper.AddIgnoreList(ref Permissions);

            if (Permissions.Length != 0)
            {
                Permissions = Permissions.Substring(0, Permissions.Length - 1);
            }

            if (RoleType == null)
            {
                RoleType = "UserRole_Farm";
            }

            switch (RoleType)
            {
                case "SystemRole_Farm":
                    ISession mContext = Context.Open();
                    mContext.Clear();
                    RolesList_FarmTbl _SystemRole_Farm = new RolesList_FarmTbl()
                    {
                        jName = RoleName,
                        uId = Encryption.GenerateAlphabicUId(),
                        Permissions = Permissions,
                        Comment = RoleComment
                    };
                    int SavedRoleId = (int)mContext.Save(_SystemRole_Farm);
                    int CurrentRoleId = Helper.Helper.getCurrentRoleId();
                    if(Helper.Helper.isStaff())
                    {
                        Helper.Helper.SaveParentChild(CurrentRoleId, (int)Helper.Helper.RoleType.RolesList_Staff, SavedRoleId, (int)Helper.Helper.RoleType.RolesList_Farm);
                    }
                    else
                    {
                        Helper.Helper.SaveParentChild(CurrentRoleId, (int)Helper.Helper.RoleType.RolesList_User, SavedRoleId, (int)Helper.Helper.RoleType.RolesList_Farm);
                    }
                    Context.Close(mContext);
                    break;

                case "SystemRole_Staff":
                    mContext = Context.Open();
                    mContext.Clear();
                    RolesList_StaffTbl _SystemRole_Staff = new RolesList_StaffTbl()
                    {
                        jName = RoleName,
                        uId = Encryption.GenerateAlphabicUId(),
                        Permissions = Permissions,
                        Comment = RoleComment
                    };
                    SavedRoleId = (int)mContext.Save(_SystemRole_Staff);
                    CurrentRoleId = Helper.Helper.getCurrentRoleId();
                    if (Helper.Helper.isStaff())
                    {
                        Helper.Helper.SaveParentChild(CurrentRoleId, (int)Helper.Helper.RoleType.RolesList_Staff, SavedRoleId, (int)Helper.Helper.RoleType.RolesList_Staff);
                    }
                    else
                    {
                        Helper.Helper.SaveParentChild(CurrentRoleId, (int)Helper.Helper.RoleType.RolesList_User, SavedRoleId, (int)Helper.Helper.RoleType.RolesList_Staff);
                    }
                    Context.Close(mContext);
                    break;

                case "UserRole_Farm":
                    mContext = Context.Open();
                    mContext.Clear();
                    RolesList_UserTbl _UserRoles = new RolesList_UserTbl()
                    {
                        jName = RoleName,
                        uId = Encryption.GenerateAlphabicUId(),
                        Permissions = Permissions,
                        Comment = RoleComment,
                        FarmId = Convert.ToInt16(FarmId)
                    };
                    SavedRoleId = (int)mContext.Save(_UserRoles);
                    CurrentRoleId = Helper.Helper.getCurrentRoleId();
                    if (Helper.Helper.isStaff())
                    {
                        Helper.Helper.SaveParentChild(CurrentRoleId, (int)Helper.Helper.RoleType.RolesList_Staff, SavedRoleId, (int)Helper.Helper.RoleType.RolesList_Farm);
                    }
                    else
                    {
                        Helper.Helper.SaveParentChild(CurrentRoleId, (int)Helper.Helper.RoleType.RolesList_User, SavedRoleId, (int)Helper.Helper.RoleType.RolesList_Farm);
                    }
                    Context.Close(mContext);
                    break;
            }
            return "ok";
        }

        [HttpPost]
        public String RemoveStaffRole(String RoleName)
        {
            ISession mContext = Context.Open();
            String RemoveStaffRole = string.Format("DELETE FROM {0} where uId = '{1}'", "SmartCattle.RolesList_StaffTbl", RoleName);
            mContext.CreateSQLQuery(RemoveStaffRole).ExecuteUpdate();
            Context.Close(mContext);
            return "OK";
        }

        [HttpPost]
        public String RemoveSystemRole_Farm(String RoleName)
        {
            RoleName = RoleName.Replace("_Remove", "");
            ISession mContext = Context.Open();
            String RemoveSystemRole_Farm = string.Format("DELETE FROM {0} where uId = '{1}'", "SmartCattle.RolesList_FarmTbl", RoleName);
            mContext.CreateSQLQuery(RemoveSystemRole_Farm).ExecuteUpdate();
            Context.Close(mContext);
            return "OK";
        }

        [HttpPost]
        public String RemoveFarmRoleFunc(String RoleName)
        {
            ISession mContext = Context.Open();
            String RemoveSystemRole_Farm = string.Format("DELETE FROM {0} where uId = '{1}'", "SmartCattle.RolesList_UserTbl", RoleName);
            mContext.CreateSQLQuery(RemoveSystemRole_Farm).ExecuteUpdate();
            Context.Close(mContext);
            return "OK";
        }

        public ActionResult EditStaffRole(String Id)
        {
            ISession mContext = Context.Open();
            List<RolesList_StaffTbl> items = mContext.QueryOver<RolesList_StaffTbl>().Where(x => x.uId == Id).List().ToList();
            List<ActionControllerListTbl> permissionList = mContext.QueryOver<ActionControllerListTbl>().List().ToList();
            Context.Close(mContext);

            // Hide some Privileges from PermissionList
            Helper.Helper.RemoveIgnoreList(ref permissionList);

            String chechedList = "";

            for (int i = 0; i < permissionList.Count; i++)
            {
                String tmp = permissionList[i].Controller + "-" + permissionList[i].Action;
                if (items.Count != 0)
                {
                    if (items[0].Permissions.Contains(tmp))
                    {
                        permissionList[i].UniqueId = "OK";
                        chechedList += "onoffswitch" + permissionList[i].ID.ToString() + "-";
                    }
                    else
                    {
                        permissionList[i].UniqueId = "NO";
                    }
                }
                else
                {
                    return RedirectToAction("CreateRole");
                }

            }
            return View(permissionList);
        }

        public ActionResult EditFarmRoleFunc(String jq)
        {
            ISession mContext = Context.Open();
            List<RolesList_UserTbl> items = mContext.QueryOver<RolesList_UserTbl>().Where(x => x.uId == jq).List().ToList();
            List<ActionControllerListTbl> permissionList = mContext.QueryOver<ActionControllerListTbl>().List().ToList();
            Context.Close(mContext);

            // Hide some Privileges from PermissionList
            Helper.Helper.RemoveIgnoreList(ref permissionList);

            String chechedList = "";

            for (int i = 0; i < permissionList.Count; i++)
            {
                String tmp = permissionList[i].Controller + "-" + permissionList[i].Action;
                if (items.Count != 0)
                {
                    if (items[0].Permissions.Contains(tmp))
                    {
                        permissionList[i].UniqueId = "OK";
                        chechedList += "onoffswitch_" + permissionList[i].Controller.ToString() + "-" + permissionList[i].Action.ToString() + ",";
                    }
                    else
                    {
                        permissionList[i].UniqueId = "NO";
                    }
                }
                else
                {
                    return RedirectToAction("CreateRole");
                }

            }
            return View(permissionList);
        }

        public ActionResult EditSystemRole_Farm(String jq)
        {
            String RoleName = jq;
            if (RoleName != "/Account/EditSystemRole_Farm")
            {
                RoleName = RoleName.Replace("/en-US", "").Replace("/fa-IR", "");
                ISession mContext = Context.Open();
                List<RolesList_FarmTbl> items = mContext.QueryOver<RolesList_FarmTbl>().Where(x => x.uId == RoleName).List().ToList();
                List<ActionControllerListTbl> permissionList = mContext.QueryOver<ActionControllerListTbl>().List().ToList();
                Context.Close(mContext);
				// Hide some Privileges from PermissionList
                Helper.Helper.RemoveIgnoreList(ref permissionList);
				String chechedList = "";
				for (int i = 0; i < permissionList.Count; i++)
                {
                    String tmp = permissionList[i].Controller + "-" + permissionList[i].Action;
                    if (items.Count != 0)
                    {
                        if (items[0].Permissions.Contains(tmp))
                        {
                            permissionList[i].UniqueId = "OK";
                            chechedList += "onoffswitch" + permissionList[i].ID.ToString() + "-";
                        }
                        else
                        {
                            permissionList[i].UniqueId = "NO";
                        }
                    }
                    else
                    {
                        return RedirectToAction("CreateRole");
                    }

                }
                return View(permissionList);
            }
            else
            {
                return RedirectToAction("CreateRole");
            }

        }

        public class ViewModel_Role
        {
            public List<RolesList_UserTbl> RoleList { get; set; }
            public List<FarmTbl> FarmList { get; set; }
            public List<SystemRole_FarmView> SystemRole_Farm { get; set; }
            public List<RolesList_StaffTbl> SystemRole_Staff { get; set; }
        }

        [HttpPost]
        public string CreateRole(CreateRoleViewModel model)
        {
            var claims = (ClaimsIdentity)HttpContext.User.Identity;
            int farmID;
            int.TryParse(claims.Claims.FirstOrDefault(c => c.Type == "FarmID").Value ?? "-1", out farmID);
            if (ModelState.IsValid)
            {
                if (model.roleId == null)
                {
                    IdentityResult result = RoleManager.Create(new UserRole() { Name = model.RoleName, Description = model.Description, FarmID = farmID });
                    if (result == IdentityResult.Success)
                    {
                        return RoleManager.FindByName(model.RoleName).Id;
                    }
                }
                else
                {
                    var role = RoleManager.FindById(model.roleId);
                    if (role != null)
                    {
                        role.Name = model.RoleName;
                        role.Description = model.Description;
                        RoleManager.Update(role);
                        return role.Id;
                    }
                }
            }
            return "";

        }

        public String UpdateSystemRole_Farm(String PermissionListStr, String RoleName)
        {
            List<Role_ParentChildTbl> retValue = new List<Role_ParentChildTbl>();
            List<Role_ParentChildTbl> tmpSave = new List<Role_ParentChildTbl>();
            RoleName = RoleName.Split('?')[1].Replace("jq=", "");
            RoleName = RoleName.Replace("Id=", "");
            PermissionListStr = PermissionListStr.Substring(0, PermissionListStr.Length - 1);
            Helper.Helper.AddIgnoreList(ref PermissionListStr);
            bool f_Continue = true;

            ISession mContext = Context.Open();
            mContext.Clear();

            RolesList_FarmTbl _RolesList_FarmTbl = mContext.QueryOver<RolesList_FarmTbl>().Where(x => x.uId == RoleName).SingleOrDefault();
            if(_RolesList_FarmTbl != null)
            {
                int CurrentRoleId = _RolesList_FarmTbl.ID;
                String Diff = Helper.Helper.getStringDiff_NotInSecond(PermissionListStr, _RolesList_FarmTbl.Permissions);
                if(!Diff.Equals(""))
                {
                    Helper.Helper.RemovePrivilege(Diff, _RolesList_FarmTbl.ID, Helper.Helper.RoleType.RolesList_Farm);
                    List<Role_ParentChildTbl> AllChildWithParentOf = Helper.Helper.getAllChildWithParentOf(CurrentRoleId);
                    if (AllChildWithParentOf.Count != 0)
                    {
                        retValue.AddRange(AllChildWithParentOf);
                        tmpSave.AddRange(AllChildWithParentOf);
                    }
                    while (f_Continue)
                    {
                        List<Role_ParentChildTbl> _tmpSave = new List<Role_ParentChildTbl>();
                        List<Role_ParentChildTbl> tmp = new List<Role_ParentChildTbl>();
                        foreach (var item in tmpSave)
                        {
                            tmp = Helper.Helper.getAllChildWithParentOf(item.ChildId);
                            if (tmp.Count != 0)
                            {
                                _tmpSave.AddRange(tmp);
                            }
                            else
                            {

                            }
                        }
                        if (_tmpSave.Count != 0)
                        {
                            retValue.AddRange(_tmpSave);
                            _tmpSave = new List<Role_ParentChildTbl>();
                            tmpSave = new List<Role_ParentChildTbl>();
                            tmpSave.AddRange(_tmpSave);
                        }
                        else
                        {
                            f_Continue = false;
                            _tmpSave = new List<Role_ParentChildTbl>();
                            tmpSave = new List<Role_ParentChildTbl>();
                        }
                    }
                }
                foreach (var item in retValue)
                {
                    Helper.Helper.RemovePrivilege(Diff, item.ParentId, (Helper.Helper.RoleType)item.ParentType);
                }
                mContext.Clear();
                _RolesList_FarmTbl = mContext.QueryOver<RolesList_FarmTbl>().Where(x => x.uId == RoleName).SingleOrDefault();
                String DiffInAnotherSide = Helper.Helper.getStringDiff_NotInSecond(_RolesList_FarmTbl.Permissions, PermissionListStr);
                if(!DiffInAnotherSide.Equals(""))
                {
                    _RolesList_FarmTbl.Permissions = _RolesList_FarmTbl.Permissions + "," + DiffInAnotherSide;
                    mContext.Update(_RolesList_FarmTbl);
                    mContext.Flush();
                }
            }
            
            return "OK";
        }

        public String UpdateFarmRole(String PermissionListStr, String RoleName)
        {
            RoleName = RoleName.Split('?')[1].Replace("jq=", "");
            RoleName = RoleName.Replace("Id=", "");
            PermissionListStr = PermissionListStr.Substring(0, PermissionListStr.Length - 1);
            Helper.Helper.AddIgnoreList(ref PermissionListStr);

            ISession mContext = Context.Open();
            mContext.Clear();

            List<Role_ParentChildTbl> retValue = new List<Role_ParentChildTbl>();
            List<Role_ParentChildTbl> tmpSave = new List<Role_ParentChildTbl>();
            bool f_Continue = true;
            mContext.Clear();

            RolesList_UserTbl _RolesList_UserTbl = mContext.QueryOver<RolesList_UserTbl>().Where(x => x.uId == RoleName).SingleOrDefault();
            if (_RolesList_UserTbl != null)
            {
                int CurrentRoleId = _RolesList_UserTbl.ID;
                String Diff = Helper.Helper.getStringDiff_NotInSecond(PermissionListStr, _RolesList_UserTbl.Permissions);
                if (!Diff.Equals(""))
                {
                    Helper.Helper.RemovePrivilege(Diff, _RolesList_UserTbl.ID, Helper.Helper.RoleType.RolesList_User);
                    List<Role_ParentChildTbl> AllChildWithParentOf = Helper.Helper.getAllChildWithParentOf(CurrentRoleId);
                    if (AllChildWithParentOf.Count != 0)
                    {
                        retValue.AddRange(AllChildWithParentOf);
                        tmpSave.AddRange(AllChildWithParentOf);
                    }
                    while (f_Continue)
                    {
                        List<Role_ParentChildTbl> _tmpSave = new List<Role_ParentChildTbl>();
                        List<Role_ParentChildTbl> tmp = new List<Role_ParentChildTbl>();
                        foreach (var item in tmpSave)
                        {
                            tmp = Helper.Helper.getAllChildWithParentOf(item.ChildId);
                            if (tmp.Count != 0)
                            {
                                _tmpSave.AddRange(tmp);
                            }
                            else
                            {

                            }
                        }
                        if (_tmpSave.Count != 0)
                        {
                            retValue.AddRange(_tmpSave);
                            _tmpSave = new List<Role_ParentChildTbl>();
                            tmpSave = new List<Role_ParentChildTbl>();
                            tmpSave.AddRange(_tmpSave);
                        }
                        else
                        {
                            f_Continue = false;
                            _tmpSave = new List<Role_ParentChildTbl>();
                            tmpSave = new List<Role_ParentChildTbl>();
                        }
                    }
                }
                foreach (var item in retValue)
                {
                    Helper.Helper.RemovePrivilege(Diff, item.ChildId, (Helper.Helper.RoleType)item.ChildType);
                }
                mContext.Clear();
                _RolesList_UserTbl = mContext.QueryOver<RolesList_UserTbl>().Where(x => x.uId == RoleName).SingleOrDefault();
                String DiffInAnotherSide = Helper.Helper.getStringDiff_NotInSecond(_RolesList_UserTbl.Permissions, PermissionListStr);
                if (!DiffInAnotherSide.Equals(""))
                {
                    _RolesList_UserTbl.Permissions = _RolesList_UserTbl.Permissions + "," + DiffInAnotherSide;
                    mContext.Update(_RolesList_UserTbl);
                    mContext.Flush();
                }
            }
            Context.Close(mContext);

            return "OK";
        }

        public String UpdateStaffRole(String PermissionListStr, String RoleName)
        {
            RoleName = RoleName.Split('?')[1].Replace("jq=", "");
            RoleName = RoleName.Replace("Id=", "");
            PermissionListStr = PermissionListStr.Substring(0, PermissionListStr.Length - 1);
            Helper.Helper.AddIgnoreList(ref PermissionListStr);

            ISession mContext = Context.Open();
            mContext.Clear();

            List<Role_ParentChildTbl> retValue = new List<Role_ParentChildTbl>();
            List<Role_ParentChildTbl> tmpSave = new List<Role_ParentChildTbl>();
            bool f_Continue = true;

            mContext.Clear();

            RolesList_StaffTbl _RolesList_StaffTbl = mContext.QueryOver<RolesList_StaffTbl>().Where(x => x.uId == RoleName).SingleOrDefault();
            if (_RolesList_StaffTbl != null)
            {
                int CurrentRoleId = _RolesList_StaffTbl.ID;
                String Diff = Helper.Helper.getStringDiff_NotInSecond(PermissionListStr, _RolesList_StaffTbl.Permissions);
                if (!Diff.Equals(""))
                {
                    Helper.Helper.RemovePrivilege(Diff, CurrentRoleId, Helper.Helper.RoleType.RolesList_Staff);
                    List<Role_ParentChildTbl> AllChildWithParentOf = Helper.Helper.getAllChildWithParentOf(CurrentRoleId);
                    if (AllChildWithParentOf.Count != 0)
                    {
                        retValue.AddRange(AllChildWithParentOf);
                        tmpSave.AddRange(AllChildWithParentOf);
                    }
                    while (f_Continue)
                    {
                        List<Role_ParentChildTbl> _tmpSave = new List<Role_ParentChildTbl>();
                        List<Role_ParentChildTbl> tmp = new List<Role_ParentChildTbl>();
                        foreach (var item in tmpSave)
                        {
                            tmp = Helper.Helper.getAllChildWithParentOf(item.ChildId);
                            if (tmp.Count != 0)
                            {
                                _tmpSave.AddRange(tmp);
                            }
                            else
                            {

                            }
                        }
                        if (_tmpSave.Count != 0)
                        {
                            retValue.AddRange(_tmpSave);
                            _tmpSave = new List<Role_ParentChildTbl>();
                            tmpSave = new List<Role_ParentChildTbl>();
                            tmpSave.AddRange(_tmpSave);
                        }
                        else
                        {
                            f_Continue = false;
                            _tmpSave = new List<Role_ParentChildTbl>();
                            tmpSave = new List<Role_ParentChildTbl>();
                        }
                    }
                }
                foreach (var item in retValue)
                {
                    Helper.Helper.RemovePrivilege(Diff, item.ChildId, Helper.Helper.RoleType.RolesList_Staff);
                }
                mContext.Clear();
                _RolesList_StaffTbl = mContext.QueryOver<RolesList_StaffTbl>().Where(x => x.uId == RoleName).SingleOrDefault();
                String DiffInAnotherSide = Helper.Helper.getStringDiff_NotInSecond(_RolesList_StaffTbl.Permissions, PermissionListStr);
                if (!DiffInAnotherSide.Equals(""))
                {
                    _RolesList_StaffTbl.Permissions = _RolesList_StaffTbl.Permissions + "," + DiffInAnotherSide;
                    mContext.Update(_RolesList_StaffTbl);
                    mContext.Flush();
                }
            }
            Context.Close(mContext);
            return "OK";
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null)//|| !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }
                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> oath2Callback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new SmartCattleUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        public ActionResult Setting()
        {
            return View();
        }

        public ActionResult UserProfile()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }

    public class AssignRoleModel
    {
        public List<SmartCattleUser> users { get; set; }
        public List<UserRole> roles { get; set; }
    }
}