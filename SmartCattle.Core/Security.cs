using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattle.Core
{
    public class Security
    {
        private static readonly string _jwtkey="" ;

        public string JWTkey
        {
            get
            {
                return _jwtkey;
            }
        }
    }
}
