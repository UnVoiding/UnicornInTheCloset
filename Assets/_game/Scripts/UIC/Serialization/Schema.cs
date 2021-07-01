using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection.Emit;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Video;

namespace RomenoCompany
{
    public interface ICoreParam<TKey> { TKey Key { get; } }

    [Serializable]
    public abstract class BaseData
    {
        [SerializeField] public string title = null;
        [SerializeField] public Sprite icon = null;
        [SerializeField] public Sprite icon_Inactive = null;
        [SerializeField] public string description = null;

        public string Title => string.IsNullOrEmpty(title) ? "[Title not filled]" : title;
        public Sprite Icon => icon == null ? Resources.Load<Sprite>("UI/icon_not_found") : icon;
        public Sprite Icon_Inactive => icon_Inactive == null ? Resources.Load<Sprite>("UI/icon_not_found") : icon_Inactive;
        public string Description => string.IsNullOrEmpty(description) ? "[Description not filled]" : description;

        public ScriptableObject so;
    }
}
