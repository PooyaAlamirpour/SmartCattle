using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Helper
{
    public class TestClass
    {
        private static TestClass BaseClass;

        public static TestClass SayHello()
        {
            return BaseClass;
        }

        public static void JustOneUse()
        {

        }

        public void normalMethod()
        {
            SayHello().se();
        }

        private void se()
        {
            throw new NotImplementedException();
        }
    }
}