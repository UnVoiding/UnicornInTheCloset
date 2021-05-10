using System;
using System.Collections.Generic;

namespace RomenoCompany
{
    [Serializable]
    public class PlayerState
    {
        public bool nameEntered = false;
        public string name = "Roman";

        public static PlayerState CreateDefault()
        {
            return new PlayerState();
        }
    }
}