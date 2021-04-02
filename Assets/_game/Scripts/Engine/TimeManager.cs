using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Sirenix.OdinInspector;

namespace RomenoCompany
{
    public class TimeManager : StrictSingleton<TimeManager>
    {
        [                                                                       ShowInInspector] 
        private Dictionary<object, float> _times = new Dictionary<object, float>();
        
        
        public long CurrentDeviceTimestamp => new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        private static readonly DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static long getTimeSecondsNow => (long)(DateTime.UtcNow - epochStart).TotalSeconds;
        
        
        protected override void Setup()
        {
            DontDestroyOnLoad(gameObject);
        }
        
        public void SetTimeScale(object owner, float timeScale)
        {
            if (_times.ContainsKey(owner)) _times[owner] = timeScale;
            else _times.Add(owner, timeScale);
        }
    
        public float GetTimeScale(object owner)
        {
            if (_times.ContainsKey(owner)) return _times[owner];
            return 1;
        }
    
        public void RemoveOwner(object owner)
        {
            _times.Remove(owner);
        }
    
        public void Reset()
        {
            _times.Clear();
            _times.Add("standard", 1);
            Time.timeScale = 1;
        }
    
        public float TimeScale
        {
            get
            {
                float timeScale = 1f;
                    timeScale = Mathf.Min(_times.Values.ToArray());
                if(timeScale == 1)
                    timeScale = Mathf.Max(_times.Values.ToArray());
    
                return timeScale;
            }
        }
    
        public static string TimeToString(long seconds)
        {
            if(seconds >= 60)
            {
                if(seconds >= 3600)
                {
                    return $"{(int)(seconds / 3600)}H {(int)(seconds / 60) % 60}M";
                }
                else return $"{(int)(seconds / 60)}M {seconds % 60}S";
            }
            else return $"{seconds}S";
        }
    }
}
