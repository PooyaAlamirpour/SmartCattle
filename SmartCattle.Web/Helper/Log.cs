using NHibernate;
using SmartCattle.Web.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Helper
{
    public class Log
    {
        public static void Write(String Message, String Value = null)
        {
            ISession mContext = Context.Open();

            ExceptionTbl newException = new ExceptionTbl();
            newException.Message = Message;
            newException.Value = Value;
            newException.Date = DateTime.Now;

            mContext.Save(newException);

            Context.Close(mContext);
        }
    }
}