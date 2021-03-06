using Sirenix.OdinInspector;
using UnityEngine;

namespace RomenoCompany
{
    public class AudioManager : StrictSingleton<AudioManager>
    {
        // [                                                   SerializeField, FoldoutGroup("Settings")] 
        // public string _soundBusName = "[SOUND]";

        protected override void Setup()
        {
            var audioStatus = Inventory.Instance.audioStatus;
            audioStatus.OnChange += (status) =>
            {
                ToggleMuteMusic(status.isMusicMuted);
                ToggleMuteSound(status.isSoundMuted);
            };
            ToggleMuteMusic(audioStatus.Value.isMusicMuted);
            ToggleMuteSound(audioStatus.Value.isSoundMuted);
            
            DontDestroyOnLoad(gameObject);
        }

        public void ToggleMuteMusic(bool mute)
        {
            
        }

        public void ToggleMuteSound(bool mute)
        {
            
        }

        public void PlayMenuMusic()
        {
            
        }
        
        public void PlayGameMusic()
        {
            
        }
    }
}

