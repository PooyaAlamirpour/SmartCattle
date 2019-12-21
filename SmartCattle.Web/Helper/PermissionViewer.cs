using NHibernate;
using SmartCattle.Web.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Helper
{
    public class PermissionViewer
    {
        public static bool WillBeShow(String ActionName, String Username)
        {
            bool retValue = false;

            if (Helper.getCurrentFarmId() == -1)
            {
                ISession mContext = Context.Open();
                String PermissionList = mContext.QueryOver<RolesList_StaffTbl>().Where(x => x.uId == Helper.getCurrentRoleuId()).Select(x => x.Permissions).SingleOrDefault<String>();
                Context.Close(mContext);
                if (PermissionList != null)
                {
                    String[] SplitedPermissions = PermissionList.Split(',');
                    for (int i = 0; i < SplitedPermissions.Length; i++)
                    {
                        if (SplitedPermissions[i] == ActionName)
                        {
                            retValue = true;
                            break;
                        }
                    }
                }
                else
                {

                }
            }
            else
            {
                ISession mContext = Context.Open();
                String PermissionList = "NaN";
                int CurrentUserId = Helper.getCurrentUserId();
                String RoleName = Helper.getCurrentRoleuId();
                UserInfo _UserInfo = mContext.QueryOver<UserInfo>().Where(x => x.ID == CurrentUserId).SingleOrDefault<UserInfo>();
                if (_UserInfo == null)
                {

                }
                else
                {
                    if (_UserInfo.FarmId == -1)
                    {
                        PermissionList = mContext.QueryOver<RolesList_StaffTbl>().Where(x => x.uId == RoleName).Select(x => x.Permissions).SingleOrDefault<String>();
                    }
                    else
                    {
                        PermissionList = mContext.QueryOver<RolesList_UserTbl>().Where(x => x.uId == RoleName).Where(x => x.FarmId == Helper.getCurrentFarmId()).Select(x => x.Permissions).SingleOrDefault<String>();
                    }
                }

                Context.Close(mContext);
                if (PermissionList != null)
                {
                    String[] SplitedPermissions = PermissionList.Split(',');
                    for (int i = 0; i < SplitedPermissions.Length; i++)
                    {
                        if (SplitedPermissions[i] == ActionName)
                        {
                            retValue = true;
                            break;
                        }
                    }
                }
                else
                {

                }
            }

            return retValue;
        }
    }
}