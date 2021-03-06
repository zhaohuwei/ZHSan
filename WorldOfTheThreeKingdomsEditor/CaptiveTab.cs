﻿using GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WorldOfTheThreeKingdomsEditor
{
    class CaptiveTab : BaseTab<Captive>
    {
        protected override GameObjectList GetDataList(GameScenario scen)
        {
            return scen.Captives;
        }

        protected override Dictionary<string, string> GetDefaultValues()
        {
            return new Dictionary<string, string>()
            {
                {"CaptivePerson", "-1"},
                {"CaptiveFaction", "-1" },
                {"RansomArchitecture", "-1" }
            };
        }

        protected override string[] GetRawItemOrder()
        {
            return new String[]
            {
                "ID",
                "CaptivePerson",
                "CaptiveFaction"
            };
        }

        public CaptiveTab(GameScenario scen, DataGrid dg)
        {
            init(scen, dg);
        }
    }
}
