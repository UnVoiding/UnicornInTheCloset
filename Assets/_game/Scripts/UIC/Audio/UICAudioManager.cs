using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RomenoCompany
{
    public class UICAudioManager : StrictSingleton<UICAudioManager>
    {
        [                        Header("UICAudioManager"), SerializeField, FoldoutGroup("References")] 
        public AudioSource source;

        [                                                   SerializeField, FoldoutGroup("References")] 
        public AudioClip unlockCompanionsSound;
        [                                                   SerializeField, FoldoutGroup("References")] 
        public AudioClip receiveItemSound;
        [                                                   SerializeField, FoldoutGroup("References")] 
        public AudioClip companionMessageSound;
        [                                                   SerializeField, FoldoutGroup("References")] 
        public AudioClip heroMessageSound;
        [                                                   SerializeField, FoldoutGroup("References")] 
        public AudioClip adviceSound;

        protected override void Setup()
        {
            unlockCompanionsSound.LoadAudioData();
            receiveItemSound.LoadAudioData();
            companionMessageSound.LoadAudioData();
            heroMessageSound.LoadAudioData();
            adviceSound.LoadAudioData();
        }

        public void PlayUnlockCompanionsSound()
        {
            source.PlayOneShot(unlockCompanionsSound);
        }
        
        public void PlayReceiveItemSound()
        {
            source.PlayOneShot(receiveItemSound);
        }
        
        public void PlayCompanionMessageSound()
        {
            source.PlayOneShot(companionMessageSound);
        }

        public void PlayHeroMessageSound()
        {
            source.PlayOneShot(heroMessageSound);
        }

        public void PlayAdviceSound()
        {
            source.PlayOneShot(adviceSound);
        }
    }
}