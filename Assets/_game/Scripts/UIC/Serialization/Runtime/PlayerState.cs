using System;
using System.Collections.Generic;

namespace RomenoCompany
{
    [Serializable]
    public class PlayerState
    {
        public string name = "Roman";

        public static PlayerState CreateDefault()
        {
            return new PlayerState();
        }
    }
}