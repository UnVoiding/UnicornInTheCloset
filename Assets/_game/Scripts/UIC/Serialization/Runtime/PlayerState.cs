using System;
using System.Collections.Generic;

namespace RomenoCompany
{
    [Serializable]
    public class PlayerState
    {
        public string name;

        public static PlayerState CreateDefault()
        {
            return new PlayerState();
        }
    }
}