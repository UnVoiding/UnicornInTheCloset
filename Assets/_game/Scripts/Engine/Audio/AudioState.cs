using System;

namespace RomenoCompany
{
    [Serializable]
    public class AudioState
    {
        public bool isSoundMuted = false;
        public bool isMusicMuted = false;

        public AudioState()
        {

        }

        public static AudioState CreateDefault()
        {
            return new AudioState();
        }
    }
}