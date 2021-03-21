using System;

namespace RomenoCompany
{
    [Serializable]
    public enum FTUEState
    {
        FIRST_SESSION = 0, NONE = 99
    }

    [Serializable]
    public class FTUEStages
    {
        public bool characterIconPressed = false;
    }
}