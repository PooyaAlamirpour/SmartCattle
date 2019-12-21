using SmartCattleCoreProcessor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattleCoreProcessor.Handler
{
    class CurrentValueHandler
    {
        public String _Tag { get; set; }

        public SaveValue SetTag(string _Tag)
        {
            SaveValue _save = new SaveValue();
            _save._tag = _Tag;
            return _save;
        }

        public class SaveValue
        {
            public string _tag = "";

            //public void Save(double _input)
            //{
            //    String tag_name = _tag;
            //    SmartCattleContext mContext = new SmartCattleContext();
            //    mContext.CurrentValue
            //    .Insert(x => x.FarmId == 3)
            //    .Insert(x => x.ValueName == tag_name)
            //    .Insert(x => x.Value == _input)
            //    .ToList();
            //}

            public void Update(double _input)
            {
                String tag_name = _tag;
                SmartCattleContext mContext = new SmartCattleContext();
                int has = mContext.CurrentValue.Where(x => x.ValueName == tag_name).ToList().Count();
                if(has == 0)
                {
                    DateTime _datetime = DateTime.Now;
                    mContext.CurrentValue
                        .Insert(x => x.FarmId == 3)
                        .Insert(x => x.ValueName == tag_name)
                        .Insert(x => x.Value == _input)
                        .ToList();
                }
                else
                {
                    DateTime _datetime = DateTime.Now;
                    mContext.CurrentValue
                       .Where(x => x.ValueName == tag_name)
                       .Update(x => x.Value == _input)
                       .ToList();
                }
            }
        }
    }
}
