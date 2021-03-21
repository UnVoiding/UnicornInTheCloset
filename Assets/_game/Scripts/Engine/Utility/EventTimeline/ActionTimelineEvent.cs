using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RomenoCompany
{
    [System.Serializable]
    public class ActionTimelineEvent
    {
        public string name = "";
        public float position = 0;

        [System.Serializable]
        public class UnityEventTimeline : UnityEvent<ActionTimelineEvent>
        { }
        public UnityEventTimeline callback = new UnityEventTimeline();

        public void Execute(ActionTimeline timeline, int direction)
        {
            if (direction > 0)
            {
                Debug.Log($"e: {name} p: {position}");
                callback?.Invoke(this);
            }
        }
    }
}
