using System;
using System.Collections.Generic;

namespace RomenoCompany
{
    public class PlayerProfile
    {
        public string name;
        public List<int> items;
        public List<int> advices;

        public static PlayerProfile CreateDefault()
        {
            return new PlayerProfile();
        }
    }
}