using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unirest_net;
using SmartCattle.DomainClass;
using unirest_net.http;
using SmartCattle.Core;
using unirest_net.request;
using System.Net.Http;

namespace SmartCattle.Service
{
    class CattleActivityService
    {
        BaseServices<CattleActivityState> ActivityService;
        public CattleActivityService(BaseServices<CattleActivityState> ActivityService)
        {
            this.ActivityService = ActivityService;             
        }

    

         
         
        
    }
  
}
