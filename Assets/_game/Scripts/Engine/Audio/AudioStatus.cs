using System;

namespace RomenoCompany
{
    [Serializable]
    public class AudioStatus
    {
        public bool isSoundMuted = false;
        public bool isMusicMuted = false;

        public AudioStatus()
        {

        }

        public static AudioStatus CreateDefault()
        {
            return new AudioStatus();
        }
    }
}