﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActiveCitizen.Common.Attributes;

namespace ActiveCitizen.Model.StaticContent.Result
{
    [Available(Device = DeviceType.WebSite)]
    class StatisticSection : BaseSection
    {
        public override string Title
        {
            get
            {
                return "Итоги голосования";
            }
       }
    }
}
