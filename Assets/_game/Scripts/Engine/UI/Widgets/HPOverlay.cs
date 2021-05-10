using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace RomenoCompany
{
    public interface IWorldUI
    {
        void Process(Camera camera, HPOverlay parent);
    }

    public enum WorldUIKey
    {
        None,
        PlayerBar,
        EnemyBar,
        AbsorbingShieldPlayerBar,
        PlayerCounter
    }

    public class HPOverlay : Widget
    {
        [SerializeField, ReadOnly] 
        private List<IWorldUI> _elements = new List<IWorldUI>();

        //private Canvas rootCanvas = null;

        [System.Serializable]
        public class Container
        {
            public WorldUIKey _key = WorldUIKey.None;
            public HPOverlayBaseObject _prefab = null;

            // optimization
            public int GetPoolSize()
            {
                // switch (_key)
                // {
                //     case WorldUIKey.None:
                //         return 0;
                //         break;
                //     case WorldUIKey.PlayerBar:
                //         return 1;
                //         break;
                //     case WorldUIKey.EnemyBar:
                //         if (!Runtime.Instance.isPolygon)
                //         {
                //             // return Runtime.Instance.LevelData.GetMaxPossibleEnemiesWithHealthBars();
                //         }
                //         else
                //         {
                //             return 10;
                //         }
                //         break;
                //     case WorldUIKey.AbsorbingShieldPlayerBar:
                //         if (!Runtime.Instance.isPolygon)
                //         {
                //             // return Runtime.Instance.LevelData.IsHard ? 1 : 0;
                //         }
                //         else
                //         {
                //             return 1;
                //         }
                //         break;
                //     case WorldUIKey.PlayerCounter:
                //         return 1;
                //         break;
                //     default:
                //         return 1;
                // }
                return 0;
            }
        }

        [SerializeField]
        private List<Container> _containers = new List<Container>();

        public RectTransform rectTransform => transform as RectTransform;

        public Canvas RootCanvas { get; private set; }

        public void Init()
        {
            int poolSize = 1;
            foreach (var c in _containers)
            {
                Ocean.Instance.CreatePool(c._prefab.gameObject, c.GetPoolSize());
                Ocean.Instance.PrecreateDroplets(c._prefab.gameObject, c.GetPoolSize());
            }
        }

        public override void InitializeWidget()
        {
            widgetType = WidgetType.HP_OVERLAY;
        }

        private void LateUpdate()
        {
            if (RootCanvas == null)
            {
                RootCanvas = GetComponentInParent<Canvas>();
            }

            foreach (var item in _elements)
            {
                item.Process(Eye.Instance.Camera, this);
            }
        }

        public T Add<T>(WorldUIKey key) where T : HPOverlayBaseObject
        {
            var c = _containers.Find(x => x._key == key);
            if (c != null)
            {
                var instance = Ocean.Instance.Get(c._prefab);
                var inst = instance.GameObject.GetComponent<T>();
                _elements.Add(inst);
                return inst;
            }
            return null;
        }

        public void Remove<T>(T panel) where T : HPOverlayBaseObject
        {
            _elements.Remove(panel);
            if (panel != null) Ocean.Instance.Return(panel);
        }
    }
}
